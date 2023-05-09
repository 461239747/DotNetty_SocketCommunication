using Network.Struct;
using System;

namespace NetworkHelper
{
    /// <summary>
    /// 网络消息转换帮助类
    /// 郑伟 2022-12-20
    /// </summary>
    public static class C_NetworkMessage
    {
        /// <summary>
        /// 转换网络数据流为网络消息对象
        /// </summary>
        /// <param name="_Basebyte"></param>
        /// <returns></returns>
        public static NetworkMessage ConvertMessage(this byte[] _Basebyte)
        {
            NetworkMessage _RecvMessage = new NetworkMessage();
            //长度校验
            int len = BitConverter.ToInt32(_Basebyte, 6);
            if (len == _Basebyte.Length)
            {
                _RecvMessage.HeadByte1 = _Basebyte[0];
                _RecvMessage.HeadByte2 = _Basebyte[1];
                _RecvMessage.StandardHead = _Basebyte[2];
                _RecvMessage.SenderID = _Basebyte[3];
                _RecvMessage.DestinationID = _Basebyte[4];
                _RecvMessage.GroupID = _Basebyte[5];

                #region 消息长度验证
                byte[] _lenthBytes = new byte[4];
                Array.Copy(_Basebyte, 6, _lenthBytes, 0, 4);
                _RecvMessage.LengthMark = _lenthBytes;
                #endregion

                #region 转换消息体
                byte[] _jsonBytes = new byte[len - 14];
                Array.Copy(_Basebyte, 10, _jsonBytes, 0, len - 14);
                _RecvMessage.MessageBody = _jsonBytes;
                #endregion

                _RecvMessage.EndByte1 = _Basebyte[_Basebyte.Length - 4];
                _RecvMessage.EndByte2 = _Basebyte[_Basebyte.Length - 3];

                _RecvMessage.VerificationL = _Basebyte[_Basebyte.Length - 2];
                _RecvMessage.VerificationH = _Basebyte[_Basebyte.Length - 1];
            }
            return _RecvMessage;
        }

        /// <summary>
        /// 将网络消息输出转换为 byte[] 进行网络传输
        /// </summary>
        /// <param name="_Message"></param>
        /// <returns></returns>
        public static byte[] WriteNetworkByte(this NetworkMessage _Message)
        {
            //仅拼接正文
            byte[] _MessageByte = new byte[_Message.MessageBody.Length + 12];
            _MessageByte[0] = _Message.HeadByte1;
            _MessageByte[1] = _Message.HeadByte2;
            _MessageByte[2] = _Message.StandardHead;
            _MessageByte[3] = _Message.SenderID;
            _MessageByte[4] = _Message.DestinationID;
            _MessageByte[5] = _Message.GroupID;

            Array.Copy(_Message.LengthMark, 0, _MessageByte, 6, 4);
            Array.Copy(_Message.MessageBody, 0, _MessageByte, 10, _Message.MessageBody.Length);

            _MessageByte[_MessageByte.Length - 2] = _Message.EndByte1;
            _MessageByte[_MessageByte.Length - 1] = _Message.EndByte2;
            //插入校验位
            byte[] _WriteByte = new byte[_MessageByte.Length + 2];
            Array.Copy(_MessageByte, 0, _WriteByte, 0, _MessageByte.Length);
            _WriteByte[_WriteByte.Length - 2] = CRCHelper.CRCL(_MessageByte);
            _WriteByte[_WriteByte.Length - 1] = CRCHelper.CRCH(_MessageByte);
            return _WriteByte;
        }
    }
}
