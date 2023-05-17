using SocketAsyncUtility;
using SocketAsyncUtility.Model;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SocketTest
{
    /**
     * Author       : SPYUA.YU
     * Date         : 2020/12/12
     * Description  : MainForm
     * Reference    : Form
     * Modified     : 
     */
    public partial class MainForm : Form
    {
        #region -- 參數 --

        private ISocketServer _server;      //  Socket Server
        private ISocketClient _client;      //  Socket Client

        #endregion

        #region -- 建構 --

        /// <summary>
        ///     建構子
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //  連線按鈕底色
            btn_Listener.BackColor = Color.Red;
            btn_Connect.BackColor = Color.Red;

            //  訊息欄位賦值
            txt_ServerSend.Text = $"{(char)2}甘霖老木";
            txt_ClientSend.Text = $"{(char)3}林老師卡好";

            _server = new SocketServer();
            _server.OnConnected += SvrConnected;
            _server.OnConnectClosed += SvrConnectClosed;
            _server.OnConnectFailed += SvrConnectFailed;
            _server.OnReceived += SvrReceived;
            _server.OnSent += SvrSent;

            _client = new SocketClient();
            _client.OnConnected += CliConnected;
            _client.OnConnectClosed += CliConnectClosed;
            _client.OnConnectFailed += CliConnectFailed;
            _client.OnReceived += CliReceived;
            _client.OnSent += CliSent;

            //GGS();
            //GGC();
        }

        private async void GGS()
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Parse(txt_ServerIp.Text), (int)nud_ServerPort.Value));
            listener.Listen(2);

            var buffer = new byte[102400];

            while (true)
            {
                var client = await listener.AcceptAsync();

                while (true)
                {
                    var rcvSize = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                    if (rcvSize == 0)
                        break;
                }

                client.Shutdown(SocketShutdown.Both);
                client.Dispose();
                client.Close();
            }
        }

        private async void GGC()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            await socket.ConnectAsync(new IPEndPoint(IPAddress.Parse(txt_ClientIp.Text), (int)nud_ClientPort.Value));

            socket.Shutdown(SocketShutdown.Both);
            socket.Dispose();
            socket.Close();
        }

        #endregion

        #region -- UI 事件 --

        /// <summary>
        ///     開始監聽。(Server)
        /// </summary>
        private void btn_Listener_Click(object sender, EventArgs e)
        {
            if (btn_Listener.BackColor == Color.Red)
            {
                _server.CreateListener(txt_ServerIp.Text, (int)nud_ServerPort.Value, 1);

                SetBtnState(btn_Listener, "監聽中", Color.Green);
            }
            else
            {
                _server.CloseListener();

                SetBtnState(btn_Listener, "建立監聽", Color.Red);
            }
        }

        /// <summary>
        ///     發送訊息。(Server)
        /// </summary>
        private void btn_ServerSend_Click(object sender, EventArgs e)
        {
            _server?.Send(Encoding.UTF8.GetBytes($"{txt_ServerSend.Text}"));
        }

        /// <summary>
        ///     清除紀錄。(Server)
        /// </summary>
        private void btn_ServerClean_Click(object sender, EventArgs e)
        {
            rtb_ServerInfo.Text = "";
        }

        /// <summary>
        ///     開始連線。(Client)
        /// </summary>
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (btn_Connect.BackColor == Color.Red)
            {
                _client.CreateConnect(txt_ClientIp.Text, (int)nud_ClientPort.Value);
            }
            else
            {
                _client.CloseConnect();
            }

            //GGC();
        }

        /// <summary>
        ///     發送訊息。(Client)
        /// </summary>
        private void btn_ClientSend_Click(object sender, EventArgs e)
        {
            _client?.Send(Encoding.UTF8.GetBytes($"{txt_ClientSend.Text}"));
        }

        /// <summary>
        ///    清除紀錄 。(Client)
        /// </summary>
        private void btn_ClientClean_Click(object sender, EventArgs e)
        {
            rtb_ClientInfo.Text = "";
        }

        #endregion

        #region -- 註冊委派的方法 --

        /// <summary>
        ///     連線完成。(Server)
        /// </summary>
        private void SvrConnected(SocketMonitor monitor)
        {
            WriteLog(rtb_ServerInfo, $"Server 與對象({monitor.ConnIpPort})建立連線完成！");
            WriteLog(rtb_ServerInfo, $"開始接收對象({monitor.ConnIpPort})的訊息....");
        }

        /// <summary>
        ///     連線關閉。(Server)
        /// </summary>
        private void SvrConnectClosed(string ip, int port, string closedMsg)
        {
            WriteLog(rtb_ServerInfo, $"{closedMsg}");
        }

        /// <summary>
        ///     連線異常。(Server)
        /// </summary>
        private void SvrConnectFailed(string ip, int port, string errType, string failedMsg)
        {
            WriteLog(rtb_ServerInfo, $"{errType} : {failedMsg}");
        }

        /// <summary>
        ///     接收訊息後的處理。(Server)
        /// </summary>
        private void SvrReceived(SocketMonitor monitor, byte[] bytes, int rcvSize)
        {
            var str = Encoding.UTF8.GetString(bytes).Split('\r')[0];
            WriteLog(rtb_ServerInfo, $"接收訊息({monitor.ConnIpPort})：{str}");
        }

        /// <summary>
        ///     發送訊息後的處理。(Server)
        /// </summary>
        private void SvrSent(SocketMonitor monitor, byte[] bytes, int sndSize)
        {
        }

        /// <summary>
        ///     連線完成。(Client)
        /// </summary>
        private void CliConnected(SocketMonitor monitor)
        {
            WriteLog(rtb_ClientInfo, $"與對象({monitor.ConnIpPort}) 建立連線完成！");
            WriteLog(rtb_ClientInfo, $"開始接收對象({monitor.ConnIpPort})的訊息....");
            SetBtnState(btn_Connect, "連線中", Color.Green);
        }

        /// <summary>
        ///     連線關閉。(Client)
        /// </summary>
        private void CliConnectClosed(string ip, int port, string closedMsg)
        {
            WriteLog(rtb_ClientInfo, $"{closedMsg}");
            SetBtnState(btn_Connect, "建立連線", Color.Red);
        }

        /// <summary>
        ///     連線異常。(Client)
        /// </summary>
        private void CliConnectFailed(string ip, int port, string errType, string failedMsg)
        {
            WriteLog(rtb_ClientInfo, $"{errType} : {failedMsg}");
            SetBtnState(btn_Connect, "建立連線", Color.Red);
        }

        /// <summary>
        ///     接收訊息後的處理。(Client)
        /// </summary>
        private void CliReceived(SocketMonitor monitor, byte[] bytes, int rcvSize)
        {
            var str = Encoding.UTF8.GetString(bytes).Split('\r')[0];
            WriteLog(rtb_ClientInfo, $"接收訊息({monitor.ConnIpPort})：{str}");
        }

        /// <summary>
        ///     發送訊息後的處理。(Client)
        /// </summary>
        private void CliSent(SocketMonitor monitor, byte[] bytes, int sndSize)
        {
        }

        #endregion

        #region -- 邏輯方法 --

        /// <summary>
        ///     寫入紀錄。
        /// </summary>
        /// <param name="rtb"> RichTextBox。 </param>
        /// <param name="txt"> 紀錄內容。 </param>
        private void WriteLog(RichTextBox rtb, string txt)
        {
            Invoke(new Action(() => {
                if (rtb.Lines.Length > 10)
                    rtb.Text = string.Empty;

                rtb.AppendText($"{txt}\n");
                rtb.SelectionStart = rtb.TextLength;
                rtb.ScrollToCaret();
            }));
        }

        /// <summary>
        ///     設置連線按鈕。(通用)
        /// </summary>
        /// <param name="btn"> 連線按鈕。 </param>
        /// <param name="text"> 按鈕文字。 </param>
        /// <param name="color"> 按鈕底色。 </param>
        private void SetBtnState(Button btn, string text, Color color)
        {
            Invoke(new Action(() => {
                btn.Text = text;
                btn.BackColor = color;
            }));
        }

        #endregion
    }
}
