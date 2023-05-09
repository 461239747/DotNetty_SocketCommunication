using NetworkSocket;

namespace DotNetty_SocketCommunication.NetworkServer
{
    /// <summary>
    /// TCP服务端模块
    /// 郑伟 2023-02-09
    /// </summary>
    public static class ServerNetwork
    {
        /// <summary>
        /// 初始化服务端内存
        /// </summary>
        static ServerNetwork()
        {
            NettyTcpServer = new NettyTcpServer();
        }

        /// <summary>
        /// Sockert服务端
        /// </summary>
        public static NettyTcpServer NettyTcpServer;
    }
}
