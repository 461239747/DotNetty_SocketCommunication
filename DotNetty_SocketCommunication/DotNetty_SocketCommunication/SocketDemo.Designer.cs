namespace DotNetty_SocketCommunication
{
    partial class SocketDemo
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRecvMsg = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbServerIP = new System.Windows.Forms.ComboBox();
            this.txtPort = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSendMsg = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.btnServer = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.btnClientSendMsg = new System.Windows.Forms.Button();
            this.btnConnServer = new System.Windows.Forms.Button();
            this.txtClientSendMsg = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.NumericUpDown();
            this.txtClientRecvMsg = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "端口号";
            // 
            // txtRecvMsg
            // 
            this.txtRecvMsg.Location = new System.Drawing.Point(74, 107);
            this.txtRecvMsg.Name = "txtRecvMsg";
            this.txtRecvMsg.Size = new System.Drawing.Size(304, 117);
            this.txtRecvMsg.TabIndex = 2;
            this.txtRecvMsg.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "收到消息";
            // 
            // cmbServerIP
            // 
            this.cmbServerIP.FormattingEnabled = true;
            this.cmbServerIP.Location = new System.Drawing.Point(74, 33);
            this.cmbServerIP.Name = "cmbServerIP";
            this.cmbServerIP.Size = new System.Drawing.Size(304, 20);
            this.cmbServerIP.TabIndex = 6;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(74, 66);
            this.txtPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.txtPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(304, 21);
            this.txtPort.TabIndex = 7;
            this.txtPort.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "发送消息";
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(74, 237);
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(304, 117);
            this.txtSendMsg.TabIndex = 9;
            this.txtSendMsg.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSendMsg);
            this.groupBox1.Controls.Add(this.btnServer);
            this.groupBox1.Controls.Add(this.cmbServerIP);
            this.groupBox1.Controls.Add(this.txtSendMsg);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtRecvMsg);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 396);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP服务端";
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Enabled = false;
            this.btnSendMsg.Location = new System.Drawing.Point(229, 361);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 12;
            this.btnSendMsg.Text = "发送消息";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // btnServer
            // 
            this.btnServer.Location = new System.Drawing.Point(121, 361);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(75, 23);
            this.btnServer.TabIndex = 11;
            this.btnServer.Text = "开启服务";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtServerIP);
            this.groupBox2.Controls.Add(this.btnClientSendMsg);
            this.groupBox2.Controls.Add(this.btnConnServer);
            this.groupBox2.Controls.Add(this.txtClientSendMsg);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtServerPort);
            this.groupBox2.Controls.Add(this.txtClientRecvMsg);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(446, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 396);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TCP客户端";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(74, 33);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(304, 21);
            this.txtServerIP.TabIndex = 13;
            // 
            // btnClientSendMsg
            // 
            this.btnClientSendMsg.Enabled = false;
            this.btnClientSendMsg.Location = new System.Drawing.Point(229, 361);
            this.btnClientSendMsg.Name = "btnClientSendMsg";
            this.btnClientSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnClientSendMsg.TabIndex = 12;
            this.btnClientSendMsg.Text = "发送消息";
            this.btnClientSendMsg.UseVisualStyleBackColor = true;
            this.btnClientSendMsg.Click += new System.EventHandler(this.btnClientSendMsg_Click);
            // 
            // btnConnServer
            // 
            this.btnConnServer.Location = new System.Drawing.Point(121, 361);
            this.btnConnServer.Name = "btnConnServer";
            this.btnConnServer.Size = new System.Drawing.Size(75, 23);
            this.btnConnServer.TabIndex = 11;
            this.btnConnServer.Text = "连接服务器";
            this.btnConnServer.UseVisualStyleBackColor = true;
            this.btnConnServer.Click += new System.EventHandler(this.btnConnServer_Click);
            // 
            // txtClientSendMsg
            // 
            this.txtClientSendMsg.Location = new System.Drawing.Point(74, 237);
            this.txtClientSendMsg.Name = "txtClientSendMsg";
            this.txtClientSendMsg.Size = new System.Drawing.Size(304, 117);
            this.txtClientSendMsg.TabIndex = 9;
            this.txtClientSendMsg.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "服务IP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "发送消息";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "端口号";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(74, 66);
            this.txtServerPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.txtServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(304, 21);
            this.txtServerPort.TabIndex = 7;
            this.txtServerPort.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // txtClientRecvMsg
            // 
            this.txtClientRecvMsg.Location = new System.Drawing.Point(74, 107);
            this.txtClientRecvMsg.Name = "txtClientRecvMsg";
            this.txtClientRecvMsg.Size = new System.Drawing.Size(304, 117);
            this.txtClientRecvMsg.TabIndex = 2;
            this.txtClientRecvMsg.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "收到消息";
            // 
            // SocketDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 426);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SocketDemo";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.SocketDemo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtRecvMsg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbServerIP;
        private System.Windows.Forms.NumericUpDown txtPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox txtSendMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClientSendMsg;
        private System.Windows.Forms.Button btnConnServer;
        private System.Windows.Forms.RichTextBox txtClientSendMsg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txtServerPort;
        private System.Windows.Forms.RichTextBox txtClientRecvMsg;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtServerIP;
    }
}

