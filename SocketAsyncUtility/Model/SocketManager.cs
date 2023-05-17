using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketAsyncUtility.Model
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket 監控的管理器。
     *                用於 Socket 監控的新增與移除、已及資源釋放....等管理。
     * Reference    : 
     * Modified     : 
     */
    public class SocketManager
    {
        #region -- 屬性 --

        /// <summary>
        ///     依照關鍵字輸出 ConcurrentDictionary 的項目。
        /// </summary>
        /// <param name="key"> Socket 監控的編號。 </param>
        /// <returns> Socket 監控。 </returns>
        public SocketMonitor this[string key] => _monitors[key];

        #endregion

        #region -- 參數 --

        protected ConcurrentDictionary<string, SocketMonitor> _monitors;    //  Sockte 監控的清單

        #endregion

        #region -- 建構 --

        /// <summary>
        ///     建構子。
        /// </summary>
        public SocketManager()
        {
            _monitors = new ConcurrentDictionary<string, SocketMonitor>();
        }

        #endregion

        #region -- Public 方法 --

        /// <summary>
        ///     取得 Socket 監控清單。
        /// </summary>
        /// <param name="keys"> Key 清單。 </param>
        /// <returns> Socket 監控清單。 </returns>
        public virtual SocketMonitor[] GetMonitors(params string[] keys)
        {
            if (keys.Length <= 0)
                return _monitors.Values.ToArray();

            return GetMonitors(x => Array.IndexOf(keys, x.Key) >=0).ToArray();
        }

        /// <summary>
        ///     取得 Socket 監控清單。
        /// </summary>
        /// <param name="predicate"> 條件。 </param>
        /// <returns> Socket 監控清單。 </returns>
        public virtual IEnumerable<SocketMonitor> GetMonitors(Expression<Func<KeyValuePair<string, SocketMonitor>, bool>> predicate)
        {
            return _monitors.Where(predicate.Compile())
                            .Select(x => x.Value);
        }

        /// <summary>
        ///     關閉 Socket 監控。
        /// </summary>
        /// <param name="monitor"> Socket 監控。 </param>
        /// <returns> Task。 </returns>
        public virtual async Task CloseMonitor(SocketMonitor monitor)
        {
            await monitor.Dispose();
        }

        /// <summary>
        ///     關閉 Socket 監控清單。
        /// </summary>
        /// <param name="monitors"> Socket 監控清單。 </param>
        /// <returns> Task。 </returns>
        /// <remarks> 若未指定參數就取全部。 </remarks>
        public virtual async Task CloseMonitors(params SocketMonitor[] monitors)
        {
            //  建立非同步工作清單
            var tasks = new List<Task>();
            //  取得監控清單要處理的項目
            var tempMonitors = monitors.Length > 0 ? monitors : _monitors.Values;

            //  將處理關閉的工作加入清單
            foreach (var monitor in tempMonitors)
                tasks.Add(monitor.Shutdown());

            //  等待所有關閉的工作都完成
            await Task.WhenAll(tasks);
        }

        /// <summary>
        ///     建立 Socket 監控。
        /// </summary>
        /// <param name="socket"> Socket。 </param>
        /// <param name="bufferKey"> 快取編號。 </param>
        /// <param name="socketFlags"> 指定通訊端的傳送和接收行為。 </param>
        /// <returns> Task。 </returns>
        public virtual async Task<SocketMonitor> CreateMonitor(Socket socket, int bufferKey, SocketFlags socketFlags)
        {
            var monitor = new SocketMonitor(socket, bufferKey, socketFlags);
            //  註冊 dispose 後的回呼事件，dispose 後將項目移除
            monitor.OnDispose += new Action(() => _monitors.TryRemove(monitor.ConnIpPort, out _));

            //  檢查是否重複，重複則將就連線移除
            if (_monitors.TryGetValue(monitor.ConnIpPort, out var oldMonitor))
                await CloseMonitor(oldMonitor);

            //  將連線對象加入連線清單
            _monitors.TryAdd(monitor.ConnIpPort, monitor);

            return monitor;
        }

        #endregion
    }
}
