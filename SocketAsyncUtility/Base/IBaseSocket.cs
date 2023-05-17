using SocketAsyncUtility.Model;
using System;

namespace SocketAsyncUtility.Base
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : Socket 基礎層的介面。
     * Reference    : 
     * Modified     : 
     */
    public interface IBaseSocket
    {
        /// <summary>
        ///     連線完成的事件。
        /// </summary>
        /// <remarks> SocketMonitor=Socket 監控。 </remarks>
        event Action<SocketMonitor> OnConnected;

        /// <summary>
        ///     連線關閉的事件。
        /// </summary>
        /// <remarks> string=通訊位址、int=通訊埠、string=造成關閉的描述。 </remarks>
        event Action<string, int, string> OnConnectClosed;

        /// <summary>
        ///     連線異常的事件。
        /// </summary>
        /// <remarks> string=通訊位址、int=通訊埠、string=異常的種類、string=造成異常的描述。 </remarks>
        event Action<string, int, string, string> OnConnectFailed;

        /// <summary>
        ///     接收訊息後的事件。
        /// </summary>
        /// <remarks> SocketMonitor=Socket 監控、byte[]=接收的訊息內容、int=接收的訊息長度。 </remarks>
        event Action<SocketMonitor, byte[], int> OnReceived;

        /// <summary>
        ///     發送訊息後的事件。
        /// </summary>
        /// <remarks> SocketMonitor=Socket 監控、byte[]=接收的訊息內容、int=接收的訊息長度。 </remarks>
        event Action<SocketMonitor, byte[], int> OnSent;
    }
}
