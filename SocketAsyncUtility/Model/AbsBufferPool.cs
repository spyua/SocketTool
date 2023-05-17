using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocketAsyncUtility.Model
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : 快取池。
     * Reference    : 
     * Modified     : 
     */
    public abstract class AbsBufferPool
    {
        #region -- 屬性 --

        /// <summary>
        ///     依照關鍵字取得快取。
        /// </summary>
        /// <param name="key"> 快取的編號。 </param>
        /// <returns> 快取。 </returns>
        public byte[] this[int key] => _buffers[key].Bytes;

        #endregion

        #region -- 參數 --

        protected int _bufferCnt;                                       //  快取的數量
        protected int _bufferSize;                                      //  快取的大小
        protected ConcurrentDictionary<int, SocketBuffer> _buffers;     //  快取的清單

        #endregion

        #region -- 建構 --

        /// <summary>
        ///     建構子。
        /// </summary>
        /// <param name="bufferCnt"> 快取的數量。 </param>
        /// <param name="bufferSize"> 快取的大小。 </param>
        public AbsBufferPool(
            int bufferCnt,
            int bufferSize = 102400)
        {
            _bufferCnt = bufferCnt;
            _bufferSize = bufferSize;
            _buffers = new ConcurrentDictionary<int, SocketBuffer>();

            for (var i = 1; i <= bufferCnt; i++)
                CreateBuffer(i, true);

            CheckAndRemoveIdleCache();
        }

        #endregion

        #region -- Public 方法 --

        /// <summary>
        ///     設置快取的大小。
        /// </summary>
        /// <param name="bufferSize"> 快取的大小。 </param>
        public virtual void SetBufferSize(int bufferSize)
        {
            _bufferSize = bufferSize;
        }

        /// <summary>
        ///     停止使用指定的快取。
        /// </summary>
        /// <param name="key"> 快取的編號。 </param>
        public virtual void StopUseBuffer(int key)
        {
            if (_buffers.TryGetValue(key, out var buffer))
                buffer.Stop();
        }

        /// <summary>
        ///     準備能使用的快取。
        /// </summary>
        /// <returns> 快取的編號。 </returns>
        public virtual async Task<int> PreparingBuffer()
        {
            int result;

            //  取未使用的第一筆快取
            var key = GetKeys(x => x.Value.IsUse == false)?.FirstOrDefault();

            //  有取得快取就回傳編號
            if (key != 0)
                result = key.Value;
            else
                result = await Task.Run(() => {
                    var keys = GetKeys(x => x.Key > _bufferCnt)?.ToArray(); //  取得不含預設的 key 清單
                    var chkKey = _bufferCnt + 1;                            //  預設以外最小的 key 值

                    if (keys.Length > 0)
                        for (var val = keys[0]; val <= keys[keys.Length - 1]; val++)
                            if (val != chkKey++)
                                break;

                    CreateBuffer(chkKey, true);
                    return chkKey;
                });

            if (!_buffers.TryGetValue(result, out var buffer))
                throw new InvalidOperationException($"The attempt to get an item(key={result}) in the ConcurrentDictionary failed");

            buffer.Start();
            return result;
        }

        #endregion

        #region -- Protected 方法 --

        /// <summary>
        ///     檢查及移除未使用的快取。
        /// </summary>
        protected virtual void CheckAndRemoveIdleCache()
        {
            Task.Run(async () => {
                while (true)
                {
                    //  取得超過四分鐘且未使用的快取
                    var keys = GetKeys(x =>
                                    x.Key > _bufferCnt &&               //  不含預設
                                    !x.Value.IsUse &&                   //  未使用
                                    (DateTime.Now - x.Value.StopTime)   //  超時未連線
                                .TotalMilliseconds > 240000);

                    //  將快取移除
                    foreach (var key in keys)
                        _buffers.TryRemove(key, out _);

                    await Task.Delay(300000);
                }
            });
        }

        /// <summary>
        ///     建立新的快取。
        /// </summary>
        /// <param name="key"> 快取的編號。 </param>
        /// <param name="isThrow"> 是否擲回例外。 </param>
        /// <returns> true=成功、false=失敗。 </returns>
        protected virtual bool CreateBuffer(int key, bool isThrow = false)
        {
            var isOk = _buffers.TryAdd(key, new SocketBuffer(_bufferSize));

            if (isThrow && !isOk)
                throw new InvalidOperationException($"The attempt to add an item(key={key}) to the ConcurrentDictionary failed");

            return isOk;
        }

        /// <summary>
        ///     依條件取得 key 清單
        /// </summary>
        /// <param name="predicate"> 條件式。 </param>
        /// <returns> key 清單。 </returns>
        protected virtual IEnumerable<int> GetKeys(Expression<Func<KeyValuePair<int, SocketBuffer>, bool>> predicate)
        {
            return _buffers.Where(predicate.Compile())?     //  依條件找尋
                           .Select(x => x.Key)?             //  只取 key
                           .OrderBy(x => x);                //  遞增排列
        }

        #endregion

        #region -- Class --

        protected class SocketBuffer : IDisposable
        {
            #region -- 屬性 --

            /// <summary>
            ///     是否使用。
            /// </summary>
            public bool IsUse { get; private set; }

            /// <summary>
            ///     快取。
            /// </summary>
            public byte[] Bytes { get; private set; }

            /// <summary>
            ///     停止的時間。
            /// </summary>
            public DateTime StopTime { get; private set; }

            #endregion

            #region -- 建構 --

            /// <summary>
            ///     建構子。
            /// </summary>
            /// <param name="size"> 快取的大小。 </param>
            public SocketBuffer(int size)
                => Bytes = new byte[size];

            #endregion

            #region -- Public 方法 --

            /// <summary>
            ///     開始使用。
            /// </summary>
            public void Start()
                => IsUse = true;

            /// <summary>
            ///     停止使用。
            /// </summary>
            public void Stop()
            {
                IsUse = default;
                StopTime = DateTime.Now;

                Array.Clear(Bytes, 0, Bytes.Length);
            }

            /// <summary>
            ///     釋放資源。
            /// </summary>
            public void Dispose()
            {
                Stop();
                Bytes = default;
            }

            #endregion
        }

        #endregion
    }
}
