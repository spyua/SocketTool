using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketAsyncUtility.Model
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket 監控。
     * Reference    : 
     * Modified     : 
     */
    public class SocketMonitor
    {
        #region -- 委派 --

        public event Action OnDispose;

        #endregion

        #region -- 屬性 --

        /// <summary>
        ///     通訊資訊。
        /// </summary>
        public string ConnIpPort { get; protected set; }

        /// <summary>
        ///     通訊位址。
        /// </summary>
        public string ConnIp { get; protected set; }

        /// <summary>
        ///     通訊埠。
        /// </summary>
        public int ConnPort { get; protected set; }

        /// <summary>
        ///     快取編號。
        /// </summary>
        /// <remarks> 對應快取池提供使用的快取。 </remarks>
        public int BufferKey { get; protected set; }

        /// <summary>
        ///     連線狀態。
        /// </summary>
        /// <remarks> true=連線、false=斷線。 </remarks>
        public bool IsConn { get => _socket != default && _socket.Connected; }

        /// <summary>
        ///     指定通訊端的傳送和接收行為。
        /// </summary>
        public SocketFlags Flags { get; protected set; }

        /// <summary>
        ///     接收訊息的工作取消令牌。
        /// </summary>
        public CancellationTokenSource RcvTokenSource { get; set; }

        #endregion

        #region -- 參數 --

        protected Socket _socket;       //  Socket
        protected bool _isShutdown;     //  是否關閉連線

        #endregion

        #region -- 建構 --

        /// <summary>
        ///     建構子。
        /// </summary>
        /// <param name="socket"> Socket。 </param>
        /// <param name="bufferKey"> 快取編號。 </param>
        /// <param name="socketFlags"> 指定通訊端的傳送和接收行為。 </param>
        public SocketMonitor(
            Socket socket,
            int bufferKey,
            SocketFlags socketFlags)
        {
            //  設置參數
            _socket = socket;

            //  解析通訊資訊
            var ipEndPoint = (IPEndPoint)socket.RemoteEndPoint;

            //  設置屬性
            ConnIp = $"{ipEndPoint.Address}";
            ConnPort = ipEndPoint.Port;
            ConnIpPort = $"{ConnIp}:{ConnPort}";
            BufferKey = bufferKey;
            Flags = socketFlags;
        }

        #endregion

        #region -- Public 方法 --

        /// <summary>
        ///     接收訊息。
        /// </summary>
        /// <param name="buffer"> 接收訊息的快取。 </param>
        /// <param name="socketFlags"> 指定通訊端的傳送和接收行為。 </param>
        /// <returns> 接收內容的大小。 </returns>
        public virtual async Task<int> RcvAsync(byte[] buffer)
        {
            return await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), Flags);
        }

        /// <summary>
        ///     發送訊息。
        /// </summary>
        /// <param name="buffer"> 發送訊息的快取。 </param>
        /// <param name="socketFlags"> 指定通訊端的傳送和接收行為。 </param>
        /// <returns> 發送內容的大小。 </returns>
        public virtual async Task<int> SndAsync(byte[] bytes)
        {
            return await _socket.SendAsync(new ArraySegment<byte>(bytes), Flags);
        }

        /// <summary>
        ///     釋放資源。
        /// </summary>
        /// <returns> Task。 </returns>
        public virtual async Task Dispose()
        {
            if (!_isShutdown)
                await Shutdown();

            _socket?.Dispose();
            _socket?.Close();

            OnDispose?.Invoke();
        }

        /// <summary>
        ///     關閉連線。
        /// </summary>
        public virtual async Task Shutdown()
        {
            RcvTokenSource?.Cancel();

            //  等待延遲，讓其他非同步能夠結束
            await Task.Delay(100);

            _isShutdown = true;

            try
            {
                _socket?.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                /*
                 *  避免連線尚未關閉時又重複操作造成例外，忽略此例外。
                 *  例：
                 *      在 UI 介面上，按住 開 / 關 的按鈕快速的重複連、斷線，會出現此例外。
                 */
            }
        }

        /// <summary>
        ///     建立工作取消令牌。
        /// </summary>
        public virtual void NewRcvTokenSource()
        {
            RcvTokenSource?.Cancel();
            RcvTokenSource = new CancellationTokenSource();
        }

        #endregion
    }
}
