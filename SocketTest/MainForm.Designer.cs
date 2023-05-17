
namespace SocketTest
{
    partial class MainForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tc_Socket = new System.Windows.Forms.TabControl();
            this.tp_SocketServer = new System.Windows.Forms.TabPage();
            this.txt_ServerSend = new System.Windows.Forms.TextBox();
            this.btn_Listener = new System.Windows.Forms.Button();
            this.nud_ServerPort = new System.Windows.Forms.NumericUpDown();
            this.lbl_ServerIpPort = new System.Windows.Forms.Label();
            this.txt_ServerIp = new System.Windows.Forms.TextBox();
            this.btn_ServerSend = new System.Windows.Forms.Button();
            this.rtb_ServerInfo = new System.Windows.Forms.RichTextBox();
            this.tp_SocketClient = new System.Windows.Forms.TabPage();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.nud_ClientPort = new System.Windows.Forms.NumericUpDown();
            this.lbl_ClientIpPort = new System.Windows.Forms.Label();
            this.txt_ClientIp = new System.Windows.Forms.TextBox();
            this.txt_ClientSend = new System.Windows.Forms.TextBox();
            this.rtb_ClientInfo = new System.Windows.Forms.RichTextBox();
            this.btn_ClientSend = new System.Windows.Forms.Button();
            this.btn_ServerClean = new System.Windows.Forms.Button();
            this.btn_ClientClean = new System.Windows.Forms.Button();
            this.tc_Socket.SuspendLayout();
            this.tp_SocketServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ServerPort)).BeginInit();
            this.tp_SocketClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ClientPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tc_Socket
            // 
            this.tc_Socket.Controls.Add(this.tp_SocketServer);
            this.tc_Socket.Controls.Add(this.tp_SocketClient);
            this.tc_Socket.Location = new System.Drawing.Point(12, 12);
            this.tc_Socket.Name = "tc_Socket";
            this.tc_Socket.SelectedIndex = 0;
            this.tc_Socket.Size = new System.Drawing.Size(460, 237);
            this.tc_Socket.TabIndex = 0;
            // 
            // tp_SocketServer
            // 
            this.tp_SocketServer.BackColor = System.Drawing.SystemColors.Control;
            this.tp_SocketServer.Controls.Add(this.btn_ServerClean);
            this.tp_SocketServer.Controls.Add(this.txt_ServerSend);
            this.tp_SocketServer.Controls.Add(this.btn_Listener);
            this.tp_SocketServer.Controls.Add(this.nud_ServerPort);
            this.tp_SocketServer.Controls.Add(this.lbl_ServerIpPort);
            this.tp_SocketServer.Controls.Add(this.txt_ServerIp);
            this.tp_SocketServer.Controls.Add(this.btn_ServerSend);
            this.tp_SocketServer.Controls.Add(this.rtb_ServerInfo);
            this.tp_SocketServer.Location = new System.Drawing.Point(4, 22);
            this.tp_SocketServer.Name = "tp_SocketServer";
            this.tp_SocketServer.Padding = new System.Windows.Forms.Padding(3);
            this.tp_SocketServer.Size = new System.Drawing.Size(452, 211);
            this.tp_SocketServer.TabIndex = 0;
            this.tp_SocketServer.Text = "Server";
            // 
            // txt_ServerSend
            // 
            this.txt_ServerSend.Location = new System.Drawing.Point(6, 183);
            this.txt_ServerSend.Name = "txt_ServerSend";
            this.txt_ServerSend.Size = new System.Drawing.Size(359, 22);
            this.txt_ServerSend.TabIndex = 11;
            // 
            // btn_Listener
            // 
            this.btn_Listener.Location = new System.Drawing.Point(244, 6);
            this.btn_Listener.Name = "btn_Listener";
            this.btn_Listener.Size = new System.Drawing.Size(75, 22);
            this.btn_Listener.TabIndex = 10;
            this.btn_Listener.Text = "建立監聽";
            this.btn_Listener.UseVisualStyleBackColor = true;
            this.btn_Listener.Click += new System.EventHandler(this.btn_Listener_Click);
            // 
            // nud_ServerPort
            // 
            this.nud_ServerPort.Location = new System.Drawing.Point(178, 6);
            this.nud_ServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nud_ServerPort.Name = "nud_ServerPort";
            this.nud_ServerPort.Size = new System.Drawing.Size(60, 22);
            this.nud_ServerPort.TabIndex = 9;
            this.nud_ServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_ServerPort.Value = new decimal(new int[] {
            3065,
            0,
            0,
            0});
            // 
            // lbl_ServerIpPort
            // 
            this.lbl_ServerIpPort.Location = new System.Drawing.Point(6, 6);
            this.lbl_ServerIpPort.Name = "lbl_ServerIpPort";
            this.lbl_ServerIpPort.Size = new System.Drawing.Size(60, 22);
            this.lbl_ServerIpPort.TabIndex = 8;
            this.lbl_ServerIpPort.Text = "連線參數";
            this.lbl_ServerIpPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_ServerIp
            // 
            this.txt_ServerIp.Location = new System.Drawing.Point(72, 6);
            this.txt_ServerIp.Name = "txt_ServerIp";
            this.txt_ServerIp.Size = new System.Drawing.Size(100, 22);
            this.txt_ServerIp.TabIndex = 7;
            this.txt_ServerIp.Text = "127.0.0.1";
            // 
            // btn_ServerSend
            // 
            this.btn_ServerSend.Location = new System.Drawing.Point(371, 183);
            this.btn_ServerSend.Name = "btn_ServerSend";
            this.btn_ServerSend.Size = new System.Drawing.Size(75, 22);
            this.btn_ServerSend.TabIndex = 6;
            this.btn_ServerSend.Text = "發送訊息";
            this.btn_ServerSend.UseVisualStyleBackColor = true;
            this.btn_ServerSend.Click += new System.EventHandler(this.btn_ServerSend_Click);
            // 
            // rtb_ServerInfo
            // 
            this.rtb_ServerInfo.Location = new System.Drawing.Point(6, 34);
            this.rtb_ServerInfo.Name = "rtb_ServerInfo";
            this.rtb_ServerInfo.Size = new System.Drawing.Size(440, 143);
            this.rtb_ServerInfo.TabIndex = 5;
            this.rtb_ServerInfo.Text = "";
            // 
            // tp_SocketClient
            // 
            this.tp_SocketClient.BackColor = System.Drawing.SystemColors.Control;
            this.tp_SocketClient.Controls.Add(this.btn_ClientClean);
            this.tp_SocketClient.Controls.Add(this.btn_Connect);
            this.tp_SocketClient.Controls.Add(this.nud_ClientPort);
            this.tp_SocketClient.Controls.Add(this.lbl_ClientIpPort);
            this.tp_SocketClient.Controls.Add(this.txt_ClientIp);
            this.tp_SocketClient.Controls.Add(this.txt_ClientSend);
            this.tp_SocketClient.Controls.Add(this.rtb_ClientInfo);
            this.tp_SocketClient.Controls.Add(this.btn_ClientSend);
            this.tp_SocketClient.Location = new System.Drawing.Point(4, 22);
            this.tp_SocketClient.Name = "tp_SocketClient";
            this.tp_SocketClient.Padding = new System.Windows.Forms.Padding(3);
            this.tp_SocketClient.Size = new System.Drawing.Size(452, 211);
            this.tp_SocketClient.TabIndex = 1;
            this.tp_SocketClient.Text = "Client";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(244, 6);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(75, 22);
            this.btn_Connect.TabIndex = 16;
            this.btn_Connect.Text = "建立連線";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // nud_ClientPort
            // 
            this.nud_ClientPort.Location = new System.Drawing.Point(178, 6);
            this.nud_ClientPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nud_ClientPort.Name = "nud_ClientPort";
            this.nud_ClientPort.Size = new System.Drawing.Size(60, 22);
            this.nud_ClientPort.TabIndex = 15;
            this.nud_ClientPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_ClientPort.Value = new decimal(new int[] {
            3066,
            0,
            0,
            0});
            // 
            // lbl_ClientIpPort
            // 
            this.lbl_ClientIpPort.Location = new System.Drawing.Point(6, 6);
            this.lbl_ClientIpPort.Name = "lbl_ClientIpPort";
            this.lbl_ClientIpPort.Size = new System.Drawing.Size(60, 22);
            this.lbl_ClientIpPort.TabIndex = 14;
            this.lbl_ClientIpPort.Text = "連線參數";
            this.lbl_ClientIpPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_ClientIp
            // 
            this.txt_ClientIp.Location = new System.Drawing.Point(72, 6);
            this.txt_ClientIp.Name = "txt_ClientIp";
            this.txt_ClientIp.Size = new System.Drawing.Size(100, 22);
            this.txt_ClientIp.TabIndex = 13;
            this.txt_ClientIp.Text = "127.0.0.1";
            // 
            // txt_ClientSend
            // 
            this.txt_ClientSend.Location = new System.Drawing.Point(6, 183);
            this.txt_ClientSend.Name = "txt_ClientSend";
            this.txt_ClientSend.Size = new System.Drawing.Size(359, 22);
            this.txt_ClientSend.TabIndex = 12;
            // 
            // rtb_ClientInfo
            // 
            this.rtb_ClientInfo.Location = new System.Drawing.Point(6, 34);
            this.rtb_ClientInfo.Name = "rtb_ClientInfo";
            this.rtb_ClientInfo.Size = new System.Drawing.Size(440, 143);
            this.rtb_ClientInfo.TabIndex = 4;
            this.rtb_ClientInfo.Text = "";
            // 
            // btn_ClientSend
            // 
            this.btn_ClientSend.Location = new System.Drawing.Point(371, 183);
            this.btn_ClientSend.Name = "btn_ClientSend";
            this.btn_ClientSend.Size = new System.Drawing.Size(75, 22);
            this.btn_ClientSend.TabIndex = 3;
            this.btn_ClientSend.Text = "發送訊息";
            this.btn_ClientSend.UseVisualStyleBackColor = true;
            this.btn_ClientSend.Click += new System.EventHandler(this.btn_ClientSend_Click);
            // 
            // btn_ServerClean
            // 
            this.btn_ServerClean.Location = new System.Drawing.Point(371, 6);
            this.btn_ServerClean.Name = "btn_ServerClean";
            this.btn_ServerClean.Size = new System.Drawing.Size(75, 22);
            this.btn_ServerClean.TabIndex = 12;
            this.btn_ServerClean.Text = "清除紀錄";
            this.btn_ServerClean.UseVisualStyleBackColor = true;
            this.btn_ServerClean.Click += new System.EventHandler(this.btn_ServerClean_Click);
            // 
            // btn_ClientClean
            // 
            this.btn_ClientClean.Location = new System.Drawing.Point(371, 6);
            this.btn_ClientClean.Name = "btn_ClientClean";
            this.btn_ClientClean.Size = new System.Drawing.Size(75, 22);
            this.btn_ClientClean.TabIndex = 17;
            this.btn_ClientClean.Text = "清除紀錄";
            this.btn_ClientClean.UseVisualStyleBackColor = true;
            this.btn_ClientClean.Click += new System.EventHandler(this.btn_ClientClean_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.tc_Socket);
            this.Name = "MainForm";
            this.Text = "SocketTest";
            this.tc_Socket.ResumeLayout(false);
            this.tp_SocketServer.ResumeLayout(false);
            this.tp_SocketServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ServerPort)).EndInit();
            this.tp_SocketClient.ResumeLayout(false);
            this.tp_SocketClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ClientPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc_Socket;
        private System.Windows.Forms.TabPage tp_SocketServer;
        private System.Windows.Forms.RichTextBox rtb_ServerInfo;
        private System.Windows.Forms.TabPage tp_SocketClient;
        private System.Windows.Forms.RichTextBox rtb_ClientInfo;
        private System.Windows.Forms.Button btn_ClientSend;
        private System.Windows.Forms.Button btn_ServerSend;
        private System.Windows.Forms.Button btn_Listener;
        private System.Windows.Forms.NumericUpDown nud_ServerPort;
        private System.Windows.Forms.Label lbl_ServerIpPort;
        private System.Windows.Forms.TextBox txt_ServerIp;
        private System.Windows.Forms.TextBox txt_ServerSend;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.NumericUpDown nud_ClientPort;
        private System.Windows.Forms.Label lbl_ClientIpPort;
        private System.Windows.Forms.TextBox txt_ClientIp;
        private System.Windows.Forms.TextBox txt_ClientSend;
        private System.Windows.Forms.Button btn_ServerClean;
        private System.Windows.Forms.Button btn_ClientClean;
    }
}

