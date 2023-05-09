using NetworkSocket;

namespace DotNetty_SocketCommunication.NetworkClient
{
    /// <summary>
    /// 客户端网络模块
    /// 郑伟 2023-02-07
    /// </summary>
    public static class ClientNetwork
    {
        /// <summary>
        /// 初始化客户端内存
        /// </summary>
        static ClientNetwork()
        {
            NettyTcpClient = new NettyTcpClient();
        }

        /// <summary>
        /// Sockert客户端
        /// </summary>
        public static NettyTcpClient NettyTcpClient;
    }
}
