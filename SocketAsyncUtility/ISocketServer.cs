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
    public interface ISocketServer : IBaseSocket
    {
        /// <summary>
        ///     建立監聽。
        /// </summary>
        /// <param name="ip"> 通訊位址。 </param>
        /// <param name="port"> 通訊埠。 </param>
        /// <param name="maxListenCnt"> 最大連線數。 </param>
        /// <param name="bufferSize"> 快取暫存的大小。 </param>
        /// <param name="socketFlags"> 指定通訊端的傳送和接收行為。 </param>
        /// <param name="isStartRcv"> 是否開啟接收訊息。 </param>
        /// <param name="addressFamily"> 地址設定配置的執行個體。 </param>
        /// <param name="socketType"> 通訊端類型的執行個體。 </param>
        /// <param name="protocolType"> 支援的通訊協定。 </param>
        void CreateListener(
                string ip,
                int port,
                int maxListenCnt,
                int bufferSize = 102400,
                SocketFlags socketFlags = SocketFlags.None,
                bool isStartRcv = true,
                AddressFamily addressFamily = AddressFamily.InterNetwork,
                SocketType socketType = SocketType.Stream,
                ProtocolType protocolType = ProtocolType.Tcp);

        /// <summary>
        ///     關閉監聽。
        /// </summary>
        void CloseListener();

        /// <summary>
        ///     發送訊息。
        /// </summary>
        /// <param name="bytes"> 發送的內容。 </param>
        /// <param name="ipPorts"> 通訊資訊清單。 </param>
        void Send(byte[] bytes, params string[] ipPorts);

        /// <summary>
        ///     開始接收訊息。
        /// </summary>
        /// <param name="ipPorts"> 通訊資訊清單。 </param>
        void StartReceive(params string[] ipPorts);

        /// <summary>
        ///     開始接收訊息。
        /// </summary>
        /// <param name="ipPorts"> 通訊資訊清單。 </param>
        void StopReceive(params string[] ipPorts);
    }
}
