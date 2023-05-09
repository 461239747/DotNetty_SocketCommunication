using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkSocket
{
    /// <summary>
    /// Netty的异步通信客户端
    /// 参考:https://github.com/tangming579/DotNettySample
    /// </summary>
    public class NettyTcpServer
    {
        /// <summary>
        /// 是否关闭服务
        /// True服务关闭 False服务启动
        /// </summary>
        public bool ServerClosed
        {
            get
            {
                return serverClosed;
            }
            private set
            {
                if (serverClosed != value)
                {
                    serverClosed = value;
                    if (!ServerClosed)
                        ClosingArrivedEvent.Reset();
                    else
                        ClosingArrivedEvent.Set();
                }
            }
        }
        private bool serverClosed = true;

        /// <summary>
        /// 关闭侦听器事件 互斥锁
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
        public NettyTcpServer(bool autoSubByte = false)
        {
            this.autoSubByte = autoSubByte;
        }

        #region [新增] 郑伟 2023-02-10
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start(IPAddress ip, int port, Type t)
        {
            if (!ServerClosed)
            {
                ClosingArrivedEvent.Set();  // 停止侦听
            }
            else
            {
                NettyTcpParams args = new NettyTcpParams()
                {
                    ServerIP = ip,
                    ServerPort = port,
                    Backlog = 100,
                    HandleType = t
                };
                //线程池任务
                ThreadPool.QueueUserWorkItem(ThreadPoolCallback,
                    args);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start(string ip, int port, Type t)
        {
            try
            {
                if (!ServerClosed)
                {
                    ClosingArrivedEvent.Set();  // 停止侦听
                }
                else
                {
                    NettyTcpParams args = new NettyTcpParams()
                    {
                        ServerIP = IPAddress.Parse(ip),
                        ServerPort = port,
                        Backlog = 100,
                        HandleType = t
                    };
                    //线程池任务
                    ThreadPool.QueueUserWorkItem(ThreadPoolCallback,
                        args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("NettyTcpServer.Start 启动服务出现异常" + ex.StackTrace);
                //LogHelper.Fatal("NettyTcpServer.Start 启动服务出现异常" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="nettyTcpParams"></param>
        public void Start(NettyTcpParams nettyTcpParams)
        {
            if (!ServerClosed)
            {
                ClosingArrivedEvent.Set();  // 停止侦听
            }
            else
            {
                //线程池任务
                ThreadPool.QueueUserWorkItem(ThreadPoolCallback,
                    nettyTcpParams);
            }
        }

        /// <summary>
        /// 线程判断后启动服务
        /// </summary>
        /// <param name="state"></param>
        private void ThreadPoolCallback(object state)
        {
            if (state is NettyTcpParams)
            {
                NettyTcpParams Args = (NettyTcpParams)state;
                RunServerAsync(Args).Wait();
            }
        }
        #endregion

        /// <summary>
        /// [新增] 输入参数为NettyTcpParams 郑伟 2023-02-10
        /// 该服务包含消息头长度校验
        /// </summary>
        /// <param name="nettyTcpParams"></param>
        /// <returns></returns>
        private async Task RunServerAsync(NettyTcpParams nettyTcpParams)
        {
            ServerClosed = false;
            IChannel boundChannel;
            IEventLoopGroup bossGroup;
            IEventLoopGroup workerGroup;
            if (ServerSettings.UseLibuv)
            {
                var dispatcher = new DispatcherEventLoopGroup();
                bossGroup = dispatcher;
                workerGroup = new WorkerEventLoopGroup(dispatcher);
            }
            else
            {
                bossGroup = new MultithreadEventLoopGroup(1);
                workerGroup = new MultithreadEventLoopGroup();
            }
            try
            {
                var bootstap = new ServerBootstrap();
                bootstap.Group(bossGroup, workerGroup);

                if (ServerSettings.UseLibuv)
                {
                    bootstap.Channel<TcpServerChannel>();
                }
                else
                {
                    bootstap.Channel<TcpServerSocketChannel>();
                }
                bootstap
                    .Option(ChannelOption.SoBacklog, 100) //设置网络IO参数等
                #region [新增] 郑伟 2023-02-10
                    .Option(ChannelOption.SoKeepalive, true)//保持连接
                #endregion
                    //.Handler(new LoggingHandler("SRV-LSTN"))//在主线程组上设置一个打印日志的处理器
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        #region  [新增] 郑伟 2023-02-10
                        //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                        //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                        IChannelPipeline pipeline = channel.Pipeline;
                        //是否启用netty自动分包 add by ngc 2023年2月13日
                        if (autoSubByte)
                        {
                            //IdleStateHandler 心跳
                            pipeline.AddLast(new IdleStateHandler(150, 0, 0));//第一个参数为读，第二个为写，第三个为读写全部

                            //出栈消息，通过这个handler 在消息顶部加上消息的长度
                            pipeline.AddLast("framing-enc", new LengthFieldPrepender(4));
                            //入栈消息通过该Handler,解析消息的包长信息，并将正确的消息体发送给下一个处理Handler
                            pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        }
                        #endregion

                        pipeline.AddLast("echo", (IChannelHandler)Activator.CreateInstance(nettyTcpParams.HandleType));
                    }));
                boundChannel = await bootstap.BindAsync(nettyTcpParams.ServerIP, nettyTcpParams.ServerPort);
                Console.WriteLine($"Tcp开始监听{nettyTcpParams.ServerIP.ToString()}:{nettyTcpParams.ServerPort}");

                ServerClosed = false;
                ClosingArrivedEvent.Reset();
                ClosingArrivedEvent.WaitOne();
                await boundChannel.CloseAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                await Task.WhenAll(
               bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
               workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public void CloseAsync()
        {
            ServerClosed = true;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="handlerContext">目标消息管道</param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task WriteAsync(IChannelHandlerContext handlerContext, byte[] bytes)
        {
            if (ServerClosed)
            {
                throw new Exception("tcp服务没有开启");
            }
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException("bytes为空");
            }

            IByteBuffer byteBuffer = Unpooled.Buffer();
            byteBuffer.WriteBytes(bytes);

            await handlerContext.WriteAndFlushAsync(byteBuffer);
        }
    }
}
