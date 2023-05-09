using System;

using DotNetty.Transport.Channels;

using Network.Struct;

using NetworkHelper;

namespace DotNetty_SocketCommunication.NetworkClient
{
    /// <summary>
    /// 解析接收到的数据
    /// </summary>
    public class AnalysisClientDatas
    {
        /// <summary>
        /// [委托] 接收到网络消息
        /// </summary>
        public static Action<NetworkMessage> E_RecvNetworkMessage;

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
                E_RecvNetworkMessage?.Invoke(recvNetworkMessage);
            }
        }
    }
}
