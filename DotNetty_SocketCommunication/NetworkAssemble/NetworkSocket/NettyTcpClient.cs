using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace NetworkSocket
{
    /// <summary>
    /// Netty的异步通信客户端
    /// </summary>
    public class NettyTcpClient
    {
        /// <summary>
        /// [委托] TCP连接状态改变时触发
        /// 郑伟 2022-10-13
        /// </summary>
        public Action<bool> E_ConnectChanged;

        /// <summary>
        /// 是否连接到服务器
        /// [修改] 由变量改为属性 原:public bool Connected = false; 郑伟 2022-10-13
        /// </summary>
        public bool Connected
        {
            get
            {
                return _Connected ?? false;
            }
            set
            {
                if (!_Connected.HasValue || _Connected != value)
                {
                    _Connected = value;
                    E_ConnectChanged?.Invoke(Connected);
                    if (Connected)
                        ClosingArrivedEvent.Reset();
                    else
                        ClosingArrivedEvent.Set();
                }
            }
        }
        private bool? _Connected;

        /// <summary>
        /// 线程互斥锁
        /// </summary>
        private ManualResetEvent ClosingArrivedEvent = new ManualResetEvent(false);

        /// <summary>
        /// 是否启用netty自动分包
        /// </summary>
        private bool autoSubByte = false;

        /// <summary>
        /// 构造函数 add by ngc 2023年2月13日
        /// </summary>
        /// <param name="autoSubByte">是否启用netty自动分包 开启后Netty会使用自己的协议对网络消息进行二次封装 保证不丢包</param>
        public NettyTcpClient(bool autoSubByte = false)
        {
            this.autoSubByte = autoSubByte;
        }

        #region [新增] 郑伟 2023-02-10
        /// <summary>
        /// 启动客户端
        /// </summary>
        public void Start(IPAddress ip, int port, Type t)
        {
            NettyTcpParams args = new NettyTcpParams()
            {
                ServerIP = ip,
                ServerPort = port,
                Backlog = 100,
                HandleType = t
            };
            Task.Run(() => RunClientAsync(args));
        }

        /// <summary>
        /// 启动客户端
        /// </summary>
        public void Start(string ip, int port, Type t)
        {
            NettyTcpParams args = new NettyTcpParams()
            {
                ServerIP = IPAddress.Parse(ip),
                ServerPort = port,
                Backlog = 100,
                HandleType = t
            };
            Task.Run(() => RunClientAsync(args));
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="nettyTcpParams"></param>
        public void Start(NettyTcpParams nettyTcpParams)
        {
            Task.Run(() => RunClientAsync(nettyTcpParams));
        }
        #endregion

        IChannel clientChannel;
        /// <summary>
        /// [新增] IP地址为输入参数 郑伟 2023-02-07
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private async Task RunClientAsync(NettyTcpParams nettyTcpParams)
        {
            var group = new MultithreadEventLoopGroup();
            try
            {
                Bootstrap bootstrap = new Bootstrap();

                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)//不延迟，直接发送
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        #region [新增] 郑伟 2023-02-10
                        if (autoSubByte)
                        {
                            //出栈消息，通过这个handler 在消息顶部加上消息的长度
                            pipeline.AddLast("framing-enc", new LengthFieldPrepender(4));
                            //入栈消息,通过该Handler,解析消息的包长信息，并将正确的消息体发送给下一个处理Handler
                            pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                            //心跳检测每10秒进行一次读检测，如果10秒内ChannelRead()方法未被调用则触发一次userEventTrigger()方法.
                            pipeline.AddLast("idleStateHandle", new IdleStateHandler(10, 0, 0));
                        }
                        #endregion
                        pipeline.AddLast("echo", (IChannelHandler)Activator.CreateInstance(nettyTcpParams.HandleType));
                    }));
                clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(nettyTcpParams.ServerIP, nettyTcpParams.ServerPort));
                Console.WriteLine($"连接成功{nettyTcpParams.ServerIP.ToString()}:{nettyTcpParams.ServerPort}");
                Connected = true;
                ClosingArrivedEvent.WaitOne();
                await clientChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("RunClientAsync:" + ex.Message);
                Connected = false;
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
                Connected = false;
                if (!Connected)
                {
                    await ReConnect(nettyTcpParams);
                }
            }
        }

        /// <summary>
        /// 重连
        /// [新增] IP地址为输入参数 郑伟 2023-02-07
        /// </summary>
        /// <returns></returns>
        private async Task ReConnect(NettyTcpParams nettyTcpParams)
        {
            if (!Connected)
            {
                await Task.Delay(3000).ContinueWith(async task =>
                {
                    Console.WriteLine($"{nettyTcpParams.ServerIP.ToString()}:{nettyTcpParams.ServerPort}开始重连");
                    //LogHelper.Info($"{nettyTcpParams.ServerIP.ToString()}:{nettyTcpParams.ServerPort}开始重连");
                    await RunClientAsync(nettyTcpParams);
                });
            }
        }

        /// <summary>
        /// 异步写消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public async Task WriteAsync(byte[] bytes)
        {
            if (!Connected)
            {
                throw new Exception("tcp服务没有开启");
            }
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException("bytes为空");
            }

            IByteBuffer byteBuffer = Unpooled.Buffer();
            byteBuffer.WriteBytes(bytes);
            await clientChannel.WriteAndFlushAsync(byteBuffer);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public void CloseAsync()
        {
            Connected = false;
        }
    }
}
