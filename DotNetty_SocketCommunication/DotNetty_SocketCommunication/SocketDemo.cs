using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DotNetty.Transport.Channels;

using Moniter.Main;

using Network.Struct;

using NetworkHelper;

using NetworkSocket;

using Newtonsoft.Json;

using DotNetty_SocketCommunication.MessageModel;
using DotNetty_SocketCommunication.NetworkClient;
using DotNetty_SocketCommunication.NetworkServer;

namespace DotNetty_SocketCommunication
{
    public partial class SocketDemo : Form
    {
        public SocketDemo()
        {
            InitializeComponent();
        }

        private void SocketDemo_Load(object sender, EventArgs e)
        {
            #region 服务下拉框初始化
            List<string> serverIpList = AddressHelper.GetLocalIPList();
            foreach (string serverIp in serverIpList)
            {
                cmbServerIP.Items.Add(serverIp);
            }
            if (cmbServerIP.Items.Count > 0)
            {
                cmbServerIP.SelectedIndex = cmbServerIP.Items.Count - 1;
                txtServerIP.Text = cmbServerIP.Text;
            }
            #endregion
        }

        #region TCP服务端
        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnServer_Click(object sender, EventArgs e)
        {
            if (ServerNetwork.NettyTcpServer.ServerClosed)
            {
                AnalysisTeacherDatas.E_RecvNetworkMessage = Tcp_ReceiveData;
                AnalysisTeacherDatas.E_StudentClientExit = StudentOffline;
                //更多事件请在 EchoServerHandler 中查找
                IPAddress _TeacherIPAddress = IPAddress.Loopback;
                if (IPAddress.TryParse(cmbServerIP.Text, out _TeacherIPAddress))
                {
                    ServerNetwork.NettyTcpServer.Start(_TeacherIPAddress, Convert.ToInt32(txtPort.Value), typeof(EchoServerHandler));
                }
                btnServer.Text = "停止服务";
                btnSendMsg.Enabled = true;
            }
            else
            {
                ServerNetwork.NettyTcpServer.CloseAsync();
                btnServer.Text = "开启服务";
                btnSendMsg.Enabled = false;
            }
            //可以通过ServerNetwork.NettyTcpServer.ServerClosed;获得服务启动状态 这里因为是异步启动 所以直接为按键赋值
        }

        /// <summary>
        /// TCP接收数据
        /// </summary>
        private void Tcp_ReceiveData(IChannelHandlerContext context, NetworkMessage recvNetworkMessage)
        {
            string _MessageBody = StringFromByteArr(recvNetworkMessage.MessageBody);
            switch (recvNetworkMessage.StandardHead)
            {
                case StandardHeadForClient.TextMeaasge:
                    this.Invoke(new Action(() =>
                    {
                        txtRecvMsg.Text = "接收到客户端信息:" + _MessageBody;
                    }));
                    break;
                case StandardHeadForClient.ObjectMessage:
                    UserInfo _LoginUser = JsonConvert.DeserializeObject<UserInfo>(_MessageBody);
                    if (_LoginUser != null)
                    {
                        ExternalLoginClient.ExternalLogin(_LoginUser.ID, context);
                        this.Invoke(new Action(() =>
                        {
                            txtRecvMsg.Text = "客户端上线 客户端信息:" + _MessageBody;
                        }));
                    }
                    break;
            }
        }

        /// <summary>
        /// 学生客户端下线
        /// </summary>
        /// <param name="exitStudentID"></param>
        private void StudentOffline(int exitStudentID)
        {
            this.Invoke(new Action(() =>
            {
                txtRecvMsg.Text = "有客户端下线";
            }));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            string msg = txtSendMsg.Text;
            NetworkMessage networkMessage = NetworkMessageCreater.Create(StandardHeadForClient.TextMeaasge, StandardSender.Server, StandardSender.Empty, msg);
            if (ExternalLoginClient.ExternalClientDic != null)
            {
                foreach (IChannelHandlerContext item in ExternalLoginClient.ExternalClientDic.Values)
                {
                    WriteAsync(item, networkMessage);
                }
            }
        }

        /// <summary>
        /// 发送消息至单独客户端
        /// </summary>
        /// <param name="handlerContext"></param>
        /// <param name="sendNetworkMessage"></param>
        public void WriteAsync(IChannelHandlerContext handlerContext, NetworkMessage sendNetworkMessage)
        {
            ServerNetwork.NettyTcpServer.WriteAsync(handlerContext, sendNetworkMessage.WriteNetworkByte());
        }
        #endregion

        #region TCP客户端
        /// <summary>
        /// 启动TCP客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnServer_Click(object sender, EventArgs e)
        {
            //更多事件请查看EchoNettyClientHandler
            IPAddress _CoreIPAddress = IPAddress.Loopback;
            if (!ClientNetwork.NettyTcpClient.Connected)
            {
                if (IPAddress.TryParse(txtServerIP.Text, out _CoreIPAddress))
                {
                    AnalysisClientDatas.E_RecvNetworkMessage = Tcp_ReceiveData;
                    ClientNetwork.NettyTcpClient.E_ConnectChanged = Tcp_ConnectChanged;
                    ClientNetwork.NettyTcpClient.Start(_CoreIPAddress, Convert.ToInt32(txtServerPort.Value), typeof(EchoNettyClientHandler));
                }
            }
            else
            {
                ClientNetwork.NettyTcpClient.CloseAsync();
            }
        }

        /// <summary>
        /// 网络连接成功
        /// </summary>
        private void Tcp_ConnectChanged(bool _IsConn)
        {
            if (_IsConn)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.ID = 1;
                userInfo.Name = "TCP客户端";

                NetworkMessage _ConnectedMessage = NetworkMessageCreater.Create(StandardHeadForClient.ObjectMessage, StandardSender.Client1, StandardSender.Server, userInfo);
                SendToServer(_ConnectedMessage);
                this.Invoke(new MethodInvoker(delegate
                {
                    txtClientRecvMsg.Text = "网络成功连接";
                    btnConnServer.Text = "断开网络连接";
                    btnClientSendMsg.Enabled = true;
                }));
            }
            else
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    txtClientRecvMsg.Text = "网络连接断开";
                    btnConnServer.Text = "连接服务器";
                    btnClientSendMsg.Enabled = false;
                }));
            }
        }

        /// <summary>
        /// TCP接收数据
        /// </summary>
        private void Tcp_ReceiveData(NetworkMessage recvNetworkMessage)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                string _MessageBody = StringFromByteArr(recvNetworkMessage.MessageBody);
                switch (recvNetworkMessage.StandardHead)
                {
                    case StandardHeadForClient.TextMeaasge:
                        txtClientRecvMsg.Text = "接收到服务端消息" + _MessageBody;
                        //FaultRecordInfo _RecvFaultInfo = JsonHelper.JsonToObject<FaultRecordInfo>(_MessageBody);
                        //FaultRecord.InsertFaultRecord(_RecvFaultInfo);
                        break;
                }
            }));
        }

        /// <summary>
        /// 发送网络消息至服务端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClientSendMsg_Click(object sender, EventArgs e)
        {
            string SendMsg = txtClientSendMsg.Text;
            NetworkMessage networkMessage = NetworkMessageCreater.Create(StandardHeadForClient.TextMeaasge, StandardSender.Client1, StandardSender.Server, SendMsg);
            SendToServer(networkMessage);
        }

        /// <summary>
        /// 发送消息至核心端
        /// </summary>
        /// <param name="_SendNetworkMessage"></param>
        private void SendToServer(NetworkMessage _SendNetworkMessage)
        {
            ClientNetwork.NettyTcpClient.WriteAsync(_SendNetworkMessage.WriteNetworkByte());
        }
        #endregion

        /// <summary>
        /// 使用UTF-8编码转换接收到的byte数组转换为字符
        /// </summary>
        /// <param name="_bytes"></param>
        /// <returns></returns>
        public static string StringFromByteArr(byte[] _bytes)
        {
            if (_bytes.Length == 0)
                return string.Empty;
            return System.Text.ASCIIEncoding.UTF8.GetString(_bytes).TrimEnd(new char[] { ' ' });
        }
    }
}
