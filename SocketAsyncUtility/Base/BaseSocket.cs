using SocketAsyncUtility.Core;
using SocketAsyncUtility.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocketAsyncUtility.Base
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket 基礎層。
     * Reference    : 
     * Modified     : 
     */
    public abstract class BaseSocket : IBaseSocket
    {
        #region -- 委派 --

        public event Action<SocketMonitor> OnConnected;
        public event Action<string, int, string> OnConnectClosed;
        public event Action<string, int, string, string> OnConnectFailed;
        public event Action<SocketMonitor, byte[], int> OnReceived;
        public event Action<SocketMonitor, byte[], int> OnSent;

        #endregion

        #region -- 參數 --

        protected BufferPool _bufferPool;           //  接收訊息的快取池
        protected SocketManager _socketManager;     //  連線清單

        #endregion

        #region -- 建構 --

        /// <summary>
        ///     建構子。
        /// </summary>
        public BaseSocket()
        {
            _bufferPool = new BufferPool(1);
            _socketManager = new SocketManager();

            DoGcCollect();
        }

        #endregion

        #region -- Protected 方法 --

        /// <summary>
        ///     定期回收記憶體。
        /// </summary>
        protected virtual void DoGcCollect()
        {
            Task.Run(async () => {
                while (true)
                {
                    //  等待延遲
                    await Task.Delay(1000);

                    //  回收記憶體
                    GC.Collect();
                }
            });
        }

        /// <summary>
        ///     執行持續接收訊息。
        /// </summary>
        /// <param name="monitor"> Socket 監控。 </param>
        protected virtual void DoStartReceive(SocketMonitor monitor)
        {
            //  建立新的工作取消令牌
            monitor.NewRcvTokenSource();

            //  建立非同步的持續接收訊息工作
            var rcvTask = Task.Run(async () => {
                while (!monitor.RcvTokenSource.IsCancellationRequested)
                {
                    //  等待接收訊息，若接收訊息長度為 0，表示已無連線
                    if ((await monitor.RcvAsync(_bufferPool[monitor.BufferKey])).EqualTo(out var rcvSize, 0))
                        break;

                    //  調用接收訊息後的回呼
                    OnReceived?.Invoke(monitor, new ArraySegment<byte>(_bufferPool[monitor.BufferKey], 0, rcvSize).ToArray(), rcvSize);

                    Array.Clear(_bufferPool[monitor.BufferKey], 0, _bufferPool[monitor.BufferKey].Length);
                }

                //  調用連線關閉的回呼
                OnConnectClosed?.Invoke(monitor.ConnIp, monitor.ConnPort, $"The connetion is closed.");

                await CloseSocketMonitor(monitor);
            },
            monitor.RcvTokenSource.Token);

            rcvTask.ContinueWith(async t => {
                if (t.Exception != null)
                {
                    var ex = t.Exception.InnerException;

                    //  調用連線異常的回呼
                    OnConnectFailed?.Invoke(monitor.ConnIp, monitor.ConnPort, ex.GetType().Name, ex.Message);

                    await CloseSocketMonitor(monitor);
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);     //  只在 rcvTask 執行時發生異常時觸發 ContinueWith
        }

        /// <summary>
        ///     檢查通訊資訊。
        /// </summary>
        /// <param name="ip"> 通訊位址。 </param>
        /// <param name="port"> 通訊埠。 </param>
        protected virtual IPEndPoint ChkConnParams(string ip, int port)
        {
            //  檢查 ip 是否為空
            if (string.IsNullOrEmpty(ip))
                throw new Exception($"The IP is empty, please check it！");

            //  檢查 ip 格式是否正確 (範圍從 1.0.0.0 到 255.255.255.255)
            if (!new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$").IsMatch(ip, 0))
                throw new Exception($"The IP({ip}) isn't in the correct format！");

            //  檢查 port 是否正確 (範圍從 0 到 65535)
            if (!(port >= 0 && port <= 65535))
                throw new Exception($"The port({port}) isn't in the correct format！");

            //  建立連線資訊物件
            return new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        ///     Try Catch Socket 的執行。
        /// </summary>
        /// <param name="func"> 執行的委派方法。 </param>
        /// <param name="dispose"> 例外時的委派方法。 </param>
        /// <param name="final"> 最後要處理的委派方法。 </param>
        /// <returns> Task。 </returns>
        protected virtual async Task TrySocket(Func<Task> func, Action<Exception> dispose, Action final = default)
        {
            try
            {
                await func.Invoke().ConfigureAwait(false);
            }
            catch (OperationCanceledException ex)
            {
                dispose?.Invoke(ex);
            }
            catch (SocketException ex)
            {
                dispose?.Invoke(ex);
            }
            catch (Exception ex)
            {
                dispose?.Invoke(ex);
            }
            finally
            {
                final?.Invoke();
            }
        }

        /// <summary>
        ///     關閉及釋放資源。
        /// </summary>
        /// <param name="monitor"> Socket 監控。 </param>
        /// <returns> Task。 </returns>
        protected virtual async Task CloseSocketMonitor(SocketMonitor monitor)
        {
            var bufferKey = monitor.BufferKey;

            //  關閉及釋放 Socket 監控資源
            await _socketManager.CloseMonitor(monitor);

            //  關閉及釋放快取資源
            _bufferPool.StopUseBuffer(bufferKey);
        }

        /// <summary>
        ///     關閉及釋放 Socket 資源。
        /// </summary>
        /// <param name="socket"> Socket。 </param>
        /// <param name="isShutdown"> 是否要作 Shutdown。 </param>
        /// <returns> 釋放資源後的 Socket。 </returns>
        protected virtual async Task<Socket> CloseSocket(Socket socket, bool isShutdown = true)
        {
            return await Task.Run(() => {
                if (isShutdown)
                    socket?.Shutdown(SocketShutdown.Both);

                socket?.Dispose();
                socket?.Close();
                socket = default;

                return socket;
            });
        }

        #endregion

        #region -- Protected 子類別調用委派 --

        /*
         *  子類別即使繼承也無法直接調用父類別的事件委派。
         */

        protected Action<SocketMonitor> Connected() => OnConnected;
        protected Action<string, int, string> ConnectClosed() => OnConnectClosed;
        protected Action<string, int, string, string> ConnectFailed() => OnConnectFailed;
        protected Action<SocketMonitor, byte[], int> Received() => OnReceived;
        protected Action<SocketMonitor, byte[], int> Sent() => OnSent;

        #endregion

        #region -- Class --

        /// <summary>
        ///     快取池。(實作抽象類別)
        /// </summary>
        protected class BufferPool : AbsBufferPool
        {
            /// <summary>
            ///     建構子。
            /// </summary>
            /// <param name="bufferCnt"> 快取的數量。 </param>
            /// <param name="bufferSize"> 快取的大小。 </param>
            public BufferPool(
                int bufferCnt,
                int bufferSize = 102400) : base(bufferCnt, bufferSize)
            {
            }
        }

        #endregion
    }
}
