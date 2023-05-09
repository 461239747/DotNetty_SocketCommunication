using System;
using System.Net;

namespace NetworkSocket
{
    /// <summary>
    /// NettyTCP通信参数信息
    /// 郑伟 2023-02-10
    /// 参考:https://github.com/tangming579/DotNettySample
    /// </summary>
    public struct NettyTcpParams
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public IPAddress ServerIP;

        /// <summary>
        /// 通信端口
        /// </summary>
        public int ServerPort;

        /// <summary>
        /// 不明
        /// </summary>
        public int Backlog;

        /// <summary>
        /// 解析类型
        /// </summary>
        public Type HandleType;
    }
}
