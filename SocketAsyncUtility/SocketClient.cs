using SocketAsyncUtility.Base;
using SocketAsyncUtility.Core;
using System;
using System.Net.Sockets;

namespace SocketAsyncUtility
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket Client。
     *                
     *                使用方法：
     *                  1. CreateConnect -- 與對象建立連線。
     *                  2. CloseConnect --- 關閉連線。
     *                  3. Send ----------- 發送訊息。
     *                  4. StartReceive --- 開始接收訊息。
     *                  5. StopReceive ---- 停止接收訊息。
     *                  
     * Reference    : BaseSocket, ISocketClient
     * Modified     : 
     */
    public class SocketClient : BaseSocket, ISocketClient
    {
        #region -- 建構 --

        /// <summary>
        ///     建構子。
        /// </summary>
        public SocketClient() : base()
        {
        }

        #endregion

        #region -- Public 方法 --

        public virtual async void CreateConnect(
                string ip, 
                int port,
                int bufferSize = 102400,
                SocketFlags socketFlags = SocketFlags.None,
                bool isStartRcv = true,
                AddressFamily addressFamily = AddressFamily.InterNetwork,
                SocketType socketType = SocketType.Stream,
                ProtocolType protocolType = ProtocolType.Tcp)
        {
            //  建立 Socket
            var socket = new Socket(addressFamily, socketType, protocolType);
            //  檢查及建置通訊資訊
            var ipEndPoint = ChkConnParams(ip, port);
            //  建立例外委派
            var dispose = new Action<Exception>(async ex => {
                //  停止連線
                await _socketManager.CloseMonitors();
                //  調用連線關閉的回呼
                ConnectClosed()?.Invoke(ip, port, ex.Message);
            });

            await TrySocket(async () => {
                //  快取池設置新建快取時的大小
                _bufferPool.SetBufferSize(bufferSize);
                
                //  等待連線完成
                await socket.ConnectAsync(ipEndPoint);
                //  建立 Socket 監控
                var monitor = await _socketManager.CreateMonitor(socket, await _bufferPool.PreparingBuffer(), socketFlags);

                //  是否開啟接收訊息
                if (isStartRcv)
                    DoStartReceive(monitor);

                //  調用連線完成的回呼
                Connected()?.Invoke(monitor);
            },
            dispose);
        }

        public virtual async void CloseConnect()
        {
            await _socketManager.GetMonitors()[0].Shutdown();
        }

        public virtual async void Send(byte[] bytes)
        {
            if ((await _socketManager.GetMonitors()?[0].SndAsync(bytes)).BiggerThan(out var sndSize, 0))
                //  調用發送訊息後的回呼
                Sent()?.Invoke(_socketManager.GetMonitors()?[0], bytes, sndSize);
        }

        public virtual void StartReceive()
        {
            DoStartReceive(_socketManager.GetMonitors()?[0]);
        }

        public virtual void StopReceive()
        {
            _socketManager.GetMonitors()?[0].RcvTokenSource.Cancel();
        }

        #endregion
    }
}
