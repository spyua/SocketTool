using SocketAsyncUtility.Base;
using SocketAsyncUtility.Core;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketAsyncUtility
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket Server。
     *                
     *                使用方法：
     *                  1. CreateListener -- 建立監聽，等待遠端連線。
     *                  2. CloseListener --- 關閉監聽，並將連線清單中的所有連線關閉。
     *                  3. Send ------------ 發送訊息。(可指定對象，無指定則全部發送)
     *                  4. StartReceive ---- 開始接收訊息。(可指定對象，無指定則全部開始)
     *                  5. StopReceive ----- 停止接收訊息。(可指定對象，無指定則全部停止)
     *                  
     * Reference    : BaseSocket, ISocketServer
     * Modified     : 
     */
    public class SocketServer : BaseSocket, ISocketServer
    {
        #region -- 參數 --

        protected Socket _listener;                                 //  監聽
        protected CancellationTokenSource _listenTokenSource;       //  監聽的工作取消令牌

        #endregion

        #region -- 建構 --

        /// <summary>
        ///     建構子。
        /// </summary>
        public SocketServer() : base()
        {
        }

        #endregion

        #region -- Public 方法 --

        public async void CreateListener(
                string ip, 
                int port,
                int maxListenCnt,
                int bufferSize = 102400,
                SocketFlags socketFlags = SocketFlags.None,
                bool isStartRcv = true,
                AddressFamily addressFamily = AddressFamily.InterNetwork,
                SocketType socketType = SocketType.Stream,
                ProtocolType protocolType = ProtocolType.Tcp)
        {
            //  檢查及建置通訊資訊
            var ipEndPoint = ChkConnParams(ip, port);
            //  建立例外時的委派
            var dispose = new Action<Exception>(ex => {
                //  調用連線關閉的回呼
                ConnectClosed()?.Invoke(ip, port, ex.Message);
            });
            //  最後要處理的委派
            var final = new Action(async () => {
                //  停止所有連線
                await _socketManager.CloseMonitors();
                //  停止監聽
                _listener = await CloseSocket(_listener, false);
            });

            await TrySocket(async () => {
                //  建立監聽
                _listener?.Dispose();
                _listener = new Socket(addressFamily, socketType, protocolType);
                _listener.Bind(ipEndPoint);
                _listener.Listen(maxListenCnt);

                //  快取池設置新建快取時的大小
                _bufferPool.SetBufferSize(bufferSize);

                //  建立工作取消令牌
                _listenTokenSource?.Cancel();
                _listenTokenSource = new CancellationTokenSource();

                //  處理監聽
                await Task.Run(async () =>
                {
                    while (!_listenTokenSource.IsCancellationRequested)
                    {
                        //  等待連線
                        var client = await _listener.AcceptAsync().WithCancellation(_listenTokenSource.Token);
                        //  建立 Socket 監控
                        var monitor = await _socketManager.CreateMonitor(client, await _bufferPool.PreparingBuffer(), socketFlags);

                        //  是否開啟接收訊息
                        if (isStartRcv)
                            DoStartReceive(monitor);

                        //  調用連線完成的回呼
                        Connected()?.Invoke(monitor);
                    }
                },
                _listenTokenSource.Token).ConfigureAwait(false);
            },
            dispose,
            final);
        }

        public virtual void CloseListener()
        {
            _listenTokenSource?.Cancel();
        }

        public virtual void Send(byte[] bytes, params string[] ipPorts)
        {
            Task.Run(async () => {
                foreach (var monitor in _socketManager.GetMonitors(ipPorts))
                    if ((await monitor.SndAsync(bytes)).BiggerThan(out var sndSize, 0))
                        //  調用發送訊息後的回呼
                        Sent()?.Invoke(monitor, bytes, sndSize);
            });
        }

        public virtual void StartReceive(params string[] ipPorts)
        {
            foreach (var monitor in _socketManager.GetMonitors(ipPorts))
                DoStartReceive(monitor);
        }

        public virtual void StopReceive(params string[] ipPorts)
        {
            foreach (var monitor in _socketManager.GetMonitors(ipPorts))
                monitor.RcvTokenSource?.Cancel();
        }

        #endregion
    }
}
