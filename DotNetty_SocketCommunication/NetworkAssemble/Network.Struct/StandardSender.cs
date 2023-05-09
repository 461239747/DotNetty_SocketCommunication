namespace Network.Struct
{
    /// <summary>
    /// 发送者ID定义
    /// 根据需求添加
    /// </summary>
    public struct StandardSender
    {
        /// <summary>
        /// 空消息
        /// </summary>
        public const byte Empty = 0x00;

        /// <summary>
        /// 服务端
        /// </summary>
        public const byte Server = 0x01;

        /// <summary>
        /// 客户端1
        /// </summary>
        public const byte Client1 = 0x02;

        /// <summary>
        /// 客户端2
        /// </summary>
        public const byte Client2 = 0x03;
    }
}
