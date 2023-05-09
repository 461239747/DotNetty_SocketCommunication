using Network.Struct;
using Newtonsoft.Json;
using System;
using System.Text;

namespace NetworkHelper
{
    /// <summary>
    /// 网络消息创建
    /// 郑伟 2022-12-20
    /// </summary>
    public static class NetworkMessageCreater
    {
        /// <summary>
        /// 创建网络消息 空消息
        /// 创建消息中校验位不存在数据 仅在发送时才进行校验和计算
        /// </summary>
        /// <param name="_StandardHead">消息ID</param>
        /// <param name="_SenderID">源系统ID</param>
        /// <param name="_DestinationID">目标系统ID</param>
        /// <returns></returns>
        public static NetworkMessage Create(byte _StandardHead, byte _SenderID, byte _DestinationID)
        {
            NetworkMessage _Message = new NetworkMessage();
            _Message.HeadByte1 = NetworkDeploy.HeadByte1;
            _Message.HeadByte2 = NetworkDeploy.HeadByte2;
            _Message.StandardHead = _StandardHead;

            _Message.SenderID = _SenderID;
            _Message.DestinationID = _DestinationID;
            _Message.GroupID = 0x00;
            //生成消息体
            _Message.MessageBody = new byte[0];
            _Message.LengthMark = BitConverter.GetBytes(14 + _Message.MessageBody.Length);

            _Message.EndByte1 = NetworkDeploy.EndByte1;
            _Message.EndByte2 = NetworkDeploy.EndByte2;
            return _Message;
        }

        /// <summary>
        /// 创建网络消息 对象专用
        /// 创建消息中校验位不存在数据 仅在发送时才进行校验和计算
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="_StandardHead">消息ID</param>
        /// <param name="_SenderID">源系统ID</param>
        /// <param name="_DestinationID">目标系统ID</param>
        /// <param name="_Content">消息体中的内容</param>
        /// <returns></returns>
        public static NetworkMessage Create<T>(byte _StandardHead, byte _SenderID, byte _DestinationID, T _Content)
        {
            NetworkMessage _Message = new NetworkMessage();
            _Message.HeadByte1 = NetworkDeploy.HeadByte1;
            _Message.HeadByte2 = NetworkDeploy.HeadByte2;
            _Message.StandardHead = _StandardHead;

            _Message.SenderID = _SenderID;
            _Message.DestinationID = _DestinationID;
            _Message.GroupID = 0x00;
            //生成消息体
            string _JsonStr = JsonConvert.SerializeObject(_Content);
            _Message.MessageBody = _JsonStr.ByteFromString_UTF8();
            _Message.LengthMark = BitConverter.GetBytes(14 + _Message.MessageBody.Length);

            _Message.EndByte1 = NetworkDeploy.EndByte1;
            _Message.EndByte2 = NetworkDeploy.EndByte2;
            return _Message;
        }

        /// <summary>
        /// 创建网络消息 传输字节数据专用
        /// 创建消息中校验位不存在数据 仅在发送时才进行校验和计算
        /// </summary>
        /// <param name="_StandardHead">消息ID</param>
        /// <param name="_SenderID">源系统ID</param>
        /// <param name="_DestinationID">目标系统ID</param>
        /// <param name="_Content">消息体中的内容</param>
        /// <returns></returns>
        public static NetworkMessage Create(byte _StandardHead, byte _SenderID, byte _DestinationID, byte[] _Content)
        {
            NetworkMessage _Message = new NetworkMessage();
            _Message.HeadByte1 = NetworkDeploy.HeadByte1;
            _Message.HeadByte2 = NetworkDeploy.HeadByte2;
            _Message.StandardHead = _StandardHead;

            _Message.SenderID = _SenderID;
            _Message.DestinationID = _DestinationID;
            _Message.GroupID = 0x00;
            //生成消息体
            _Message.MessageBody = _Content;
            _Message.LengthMark = BitConverter.GetBytes(14 + _Message.MessageBody.Length);

            _Message.EndByte1 = NetworkDeploy.EndByte1;
            _Message.EndByte2 = NetworkDeploy.EndByte2;
            return _Message;
        }

        #region 包含小组ID
        /// <summary>
        /// 创建网络消息 空消息 包含小组ID
        /// 创建消息中校验位不存在数据 仅在发送时才进行校验和计算
        /// </summary>
        /// <param name="_StandardHead">消息ID</param>
        /// <param name="_SenderID">源系统ID</param>
        /// <param name="_DestinationID">目标系统ID</param>
        /// <param name="_GroupID">小组ID</param>
        /// <returns></returns>
        public static NetworkMessage Create(byte _StandardHead, byte _SenderID, byte _DestinationID, byte _GroupID)
        {
            NetworkMessage _Message = new NetworkMessage();
            _Message.HeadByte1 = NetworkDeploy.HeadByte1;
            _Message.HeadByte2 = NetworkDeploy.HeadByte2;
            _Message.StandardHead = _StandardHead;

            _Message.SenderID = _SenderID;
            _Message.DestinationID = _DestinationID;
            _Message.GroupID = 0x00;
            //生成消息体
            _Message.MessageBody = new byte[0];
            _Message.LengthMark = BitConverter.GetBytes(14 + _Message.MessageBody.Length);

            _Message.EndByte1 = NetworkDeploy.EndByte1;
            _Message.EndByte2 = NetworkDeploy.EndByte2;
            return _Message;
        }

        /// <summary>
        /// 创建网络消息 对象专用
        /// 创建消息中校验位不存在数据 仅在发送时才进行校验和计算
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="_StandardHead">消息ID</param>
        /// <param name="_SenderID">源系统ID</param>
        /// <param name="_DestinationID">目标系统ID</param>
        /// <param name="_GroupID">小组ID</param>
        /// <param name="_Content">消息体中的内容</param>
        /// <returns></returns>
        public static NetworkMessage Create<T>(byte _StandardHead, byte _SenderID, byte _DestinationID, byte _GroupID, T _Content)
        {
            NetworkMessage _Message = new NetworkMessage();
            _Message.HeadByte1 = NetworkDeploy.HeadByte1;
            _Message.HeadByte2 = NetworkDeploy.HeadByte2;
            _Message.StandardHead = _StandardHead;

            _Message.SenderID = _SenderID;
            _Message.DestinationID = _DestinationID;
            _Message.GroupID = 0x00;
            //生成消息体
            string _JsonStr = JsonConvert.SerializeObject(_Content);
            _Message.MessageBody = _JsonStr.ByteFromString_UTF8();
            _Message.LengthMark = BitConverter.GetBytes(14 + _Message.MessageBody.Length);

            _Message.EndByte1 = NetworkDeploy.EndByte1;
            _Message.EndByte2 = NetworkDeploy.EndByte2;
            return _Message;
        }

        /// <summary>
        /// 创建网络消息 传输字节数据专用
        /// 创建消息中校验位不存在数据 仅在发送时才进行校验和计算
        /// </summary>
        /// <param name="_StandardHead">消息ID</param>
        /// <param name="_SenderID">源系统ID</param>
        /// <param name="_DestinationID">目标系统ID</param>
        /// <param name="_GroupID">小组ID</param>
        /// <param name="_Content">消息体中的内容</param>
        /// <returns></returns>
        public static NetworkMessage Create(byte _StandardHead, byte _SenderID, byte _DestinationID, byte _GroupID, byte[] _Content)
        {
            NetworkMessage _Message = new NetworkMessage();
            _Message.HeadByte1 = NetworkDeploy.HeadByte1;
            _Message.HeadByte2 = NetworkDeploy.HeadByte2;
            _Message.StandardHead = _StandardHead;

            _Message.SenderID = _SenderID;
            _Message.DestinationID = _DestinationID;
            _Message.GroupID = 0x00;
            //生成消息体
            _Message.MessageBody = _Content;
            _Message.LengthMark = BitConverter.GetBytes(14 + _Message.MessageBody.Length);

            _Message.EndByte1 = NetworkDeploy.EndByte1;
            _Message.EndByte2 = NetworkDeploy.EndByte2;
            return _Message;
        }
        #endregion

        /// <summary>
        /// 使用ASCII编码进行字符转换
        /// </summary>
        /// <param name="_Str"></param>
        /// <returns></returns>
        public static byte[] ByteFromString_ASCII(this string _Str)
        {
            byte[] bytes = ASCIIEncoding.UTF8.GetBytes(_Str);
            return bytes;
        }

        /// <summary>
        /// 使用ASCII-UTF8编码进行字符转换
        /// </summary>
        /// <param name="_Str"></param>
        /// <returns></returns>
        public static byte[] ByteFromString_UTF8(this string _Str)
        {
            byte[] bytes = ASCIIEncoding.UTF8.GetBytes(_Str);
            return bytes;
        }
    }
}
