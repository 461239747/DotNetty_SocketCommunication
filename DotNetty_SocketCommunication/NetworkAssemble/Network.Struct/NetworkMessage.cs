using System;

namespace Network.Struct
{
    /// <summary>
    /// 网络消息对象
    /// 注意:消息体内的校验位仅在发送方法内计算赋值
    /// 郑伟 2020-12-21
    /// </summary>
    [Serializable]
    public struct NetworkMessage
    {
        /// <summary>
        /// 消息头1
        /// </summary>
        public byte HeadByte1;

        /// <summary>
        /// 消息头2
        /// </summary>
        public byte HeadByte2;

        /// <summary>
        /// 消息ID
        /// </summary>
        public byte StandardHead;

        /// <summary>
        /// 源系统ID
        /// </summary>
        public byte SenderID;

        /// <summary>
        /// 目标系统ID
        /// </summary>
        public byte DestinationID;

        /// <summary>
        /// 小组ID
        /// </summary>
        public byte GroupID;

        /// <summary>
        /// 长度验证4位
        /// </summary>
        public byte[] LengthMark;

        /// <summary>
        /// 消息体
        /// </summary>
        public byte[] MessageBody;

        /// <summary>
        /// 结束字符
        /// </summary>
        public byte EndByte1;

        /// <summary>
        /// 结束字符
        /// </summary>
        public byte EndByte2;

        /// <summary>
        /// 校验位1
        /// 校验方式按照CRC16(Modbus) 低字节在前
        /// </summary>
        public byte VerificationL;

        /// <summary>
        /// 校验位2
        /// 校验方式按照CRC16(Modbus) 高字节在后
        /// </summary>
        public byte VerificationH;

    }
}
