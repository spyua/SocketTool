using SocketAsyncUtility.Base;
using System.Net.Sockets;

namespace SocketAsyncUtility
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket Server 介面。
     * Reference    : IBaseSocket
     * Modified     : 
     */
    public interface ISocketClient : IBaseSocket
    {
        /// <summary>
        ///     建立連線。
        /// </summary>
        /// <param name="ip"> 通訊位址。 </param>
        /// <param name="port"> 通訊埠。 </param>
        /// <param name="bufferSize"> 快取暫存的大小。 </param>
        /// <param name="socketFlags"> 指定通訊端的傳送和接收行為。 </param>
        /// <param name="isStartRcv"> 是否開啟接收訊息。 </param>
        /// <param name="addressFamily"> 地址設定配置的執行個體。 </param>
        /// <param name="socketType"> 通訊端類型的執行個體。 </param>
        /// <param name="protocolType"> 支援的通訊協定。 </param>
        void CreateConnect(
                string ip,
                int port,
                int bufferSize = 102400,
                SocketFlags socketFlags = SocketFlags.None,
                bool isStartRcv = true,
                AddressFamily addressFamily = AddressFamily.InterNetwork,
                SocketType socketType = SocketType.Stream,
                ProtocolType protocolType = ProtocolType.Tcp);

        /// <summary>
        ///     關閉連線。
        /// </summary>
        void CloseConnect();

        /// <summary>
        ///     發送訊息。
        /// </summary>
        /// <param name="bytes"> 發送的內容。 </param>
        void Send(byte[] bytes);

        /// <summary>
        ///     開始接收訊息。
        /// </summary>
        /// <param name="ipPorts"> 通訊資訊清單。 </param>
        void StartReceive();

        /// <summary>
        ///     開始接收訊息。
        /// </summary>
        /// <param name="ipPorts"> 通訊資訊清單。 </param>
        void StopReceive();
    }
}
