using DotNetty.Transport.Channels;

using Network.Struct;

using NetworkHelper;

using System;

namespace DotNetty_SocketCommunication.NetworkServer
{
    /// <summary>
    /// 解析接收到的数据
    /// </summary>
    public class AnalysisTeacherDatas
    {
        /// <summary>
        /// [委托] 接收到网络消息
        /// </summary>
        public static Action<IChannelHandlerContext, NetworkMessage> E_RecvNetworkMessage;

        /// <summary>
        /// [委托] 学生客户端退出系统
        /// </summary>
        public static Action<int> E_StudentClientExit;

        /// <summary>
        /// 网络消息解析
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bytes"></param>
        public static void ProtocolAnalysis(IChannelHandlerContext context, byte[] bytes)
        {
            if (E_RecvNetworkMessage != null)
            {
                NetworkMessage recvNetworkMessage = bytes.ConvertMessage();
                E_RecvNetworkMessage?.Invoke(context, recvNetworkMessage);
            }
        }
    }
}
