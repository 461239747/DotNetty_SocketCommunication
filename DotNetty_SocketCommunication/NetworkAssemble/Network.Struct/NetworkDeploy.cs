namespace Network.Struct
{
    /// <summary>
    /// 网络消息配置
    /// 郑伟 2022-03-01
    /// </summary>
    public struct NetworkDeploy
    {
        /// <summary>
        /// 帧头1协议
        /// </summary>
        public const byte HeadByte1 = 0xFF;

        /// <summary>
        /// 帧头2协议
        /// </summary>
        public const byte HeadByte2 = 0xFF;

        /// <summary>
        /// 帧尾1协议
        /// </summary>
        public const byte EndByte1 = 0xEE;

        /// <summary>
        /// 帧尾2协议
        /// </summary>
        public const byte EndByte2 = 0xEE;
    }
}
