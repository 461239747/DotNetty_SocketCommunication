using System;

using DotNetty.Buffers;
using DotNetty.Transport.Channels;

using NetworkSocket;

using DotNetty_SocketCommunication.NetworkClient;

namespace Moniter.Main
{
    /// <summary>
    /// 实现DotNetty通信事件接口
    /// 2023-02-09
    /// </summary>
    public class EchoNettyClientHandler : ChannelHandlerAdapter
    {
        /// <summary>
        /// 消息封包处理
        /// </summary>
        NetworkPackageSpliter packetSplicingHandle = new NetworkPackageSpliter();

        readonly IByteBuffer initialMessage;
        public EchoNettyClientHandler()
        {
            initialMessage = Unpooled.Buffer(256);
            packetSplicingHandle.ProtocolHandleOverEvent += AnalysisClientDatas.ProtocolAnalysis;
            Console.WriteLine("EchoNettyClientHandler");
        }

        /// <summary>
        /// 通道激活
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context)
        {
            context.WriteAndFlushAsync(this.initialMessage);
            //GroupToTeachSendMsg.SendRequeCurrentTime(context);
            Console.WriteLine("ChannelActive" + context.Channel.RemoteAddress.ToString());
            //GroupToTeachSendMsg.SendSysInfoList();
        }

        /// <summary>
        /// 通道掉线
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            ClientNetwork.NettyTcpClient.Connected = false;
            Console.WriteLine("掉线");
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            try
            {
                var buffer = message as IByteBuffer;
                if (buffer.ReadableBytes > 0)
                {
                    byte[] messageBytes = new byte[buffer.ReadableBytes];
                    buffer.ReadBytes(messageBytes);

                    packetSplicingHandle.ReceiveBuffer(context, messageBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //LogHelper.LogError(ex.Message);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            ClientNetwork.NettyTcpClient.Connected = false;
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
            Console.WriteLine("ExceptionCaught" + context.Channel.RemoteAddress);
        }
    }
}
