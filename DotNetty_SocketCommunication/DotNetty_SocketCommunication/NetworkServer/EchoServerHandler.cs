using System;
using System.Net;

using DotNetty.Buffers;
using DotNetty.Transport.Channels;

using NetworkSocket;

namespace DotNetty_SocketCommunication.NetworkServer
{
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        /// <summary>
        /// 消息封包处理
        /// </summary>
        NetworkPackageSpliter NetworkPackageSpliter = new NetworkPackageSpliter();

        public EchoServerHandler()
        {
            NetworkPackageSpliter.ProtocolHandleOverEvent += AnalysisTeacherDatas.ProtocolAnalysis;
        }
        /// <summary>
        /// 频道注册
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            string id = context.Channel.Id.AsLongText();
            IPEndPoint iPEndPoint = (IPEndPoint)(context.Channel.RemoteAddress);
            string ip = iPEndPoint.Address.MapToIPv4().ToString();

            Console.WriteLine($"{ip}:{iPEndPoint.Port}-ChannelRegistered-id:{id}");
            //LogHelper.Info($"{ip}:{iPEndPoint.Port}-ChannelRegistered-id:{id}");
        }
        /// <summary>
        /// 频道激活
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine("ChannelActive:" + context.Channel.Id.AsLongText());
        }
        /// <summary>
        /// 客户端离线
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            string id = context.Channel.Id.AsLongText();
            IPEndPoint iPEndPoint = (IPEndPoint)(context.Channel.RemoteAddress);
            //退出登录的学生ID
            int exitStudentID = 0;
            ExternalLoginClient.ExternalExtend(context, out exitStudentID);
            if (exitStudentID > 0)
                AnalysisTeacherDatas.E_StudentClientExit?.Invoke(exitStudentID);
            Console.WriteLine($"{iPEndPoint.Address.MapToIPv4()}:{iPEndPoint.Port}-ChannelInactive-id:{id}");
            //LogHelper.Info($"{iPEndPoint.Address.MapToIPv4()}:{iPEndPoint.Port}-ChannelInactive-id:{id}掉线");
        }

        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message != null)
            {
                var buffer = message as IByteBuffer;
                if (buffer.ReadableBytes > 0)
                {
                    byte[] messageBytes = new byte[buffer.ReadableBytes];
                    buffer.ReadBytes(messageBytes);
                    NetworkPackageSpliter.ReceiveBuffer(context, messageBytes);
                }
            }
        }

        /// <summary>
        /// 通道读取完成
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            // context.CloseAsync();
            Console.WriteLine("ExceptionCaught:" + exception.Message);
            //LogHelper.Error(exception);
        }
    }
}
