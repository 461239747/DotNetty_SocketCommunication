using System;
using System.Threading;

using DotNetty.Transport.Channels;

using Network.Struct;

using NetworkHelper;

namespace NetworkSocket
{
    /// <summary>
    /// 网络消息原始数据包
    /// 郑伟 2023-02-13
    /// </summary>
    public class NetworkPackageSpliter
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        /// <param name="milliSeconds">单位:毫秒 超时限制, 如果超过设置的时间将会抛弃所有缓存数据</param>
        public NetworkPackageSpliter(int milliSeconds = -1)
        {
            if (milliSeconds >= 0)
            {
                TimeOutSpan = TimeSpan.FromMilliseconds(milliSeconds);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="timeOutSpan">超时限制, 如果超过设置的时间将会抛弃所有缓存数据</param>
        public NetworkPackageSpliter(TimeSpan timeOutSpan)
        {
            TimeOutSpan = timeOutSpan;
        }

        /// <summary>
        /// [委托] 协议处理完毕事件
        /// </summary>
        public event Action<IChannelHandlerContext, byte[]> ProtocolHandleOverEvent;

        /// <summary>
        /// 缓存网络消息
        /// </summary>
        private byte[] CacheNetworkMessage;

        /// <summary>
        /// 网络消息线程互斥锁
        /// 用于防止消息阻塞过多
        /// </summary>
        private Mutex NetworkPackageMutex = new Mutex();

        /// <summary>
        /// 超时时间
        /// 默认无限等待
        /// </summary>
        private TimeSpan TimeOutSpan = TimeSpan.FromMilliseconds(Timeout.Infinite);

        /// <summary>
        /// 接收到的网络消息
        /// </summary>
        /// <param name="handlerContext">网络消息管道</param>
        /// <param name="receiveBuffer">接收到的消息</param>
        public void ReceiveBuffer(IChannelHandlerContext handlerContext, byte[] receiveBuffer)
        {
            if (NetworkPackageMutex.WaitOne(TimeOutSpan))
            {
                #region 进行拼包处理
                if (CacheNetworkMessage != null && CacheNetworkMessage.Length > 0)
                {
                    int baseLength = CacheNetworkMessage == null ? 0 : CacheNetworkMessage.Length;
                    byte[] spliceCacheNetworkMessage = new byte[baseLength + receiveBuffer.Length];
                    Array.Copy(CacheNetworkMessage, 0, spliceCacheNetworkMessage, 0, CacheNetworkMessage.Length);
                    Array.Copy(receiveBuffer, 0, spliceCacheNetworkMessage, baseLength, receiveBuffer.Length);
                    CacheNetworkMessage = spliceCacheNetworkMessage;
                }
                else//减少申请内存空间的次数
                {
                    CacheNetworkMessage = receiveBuffer;
                }
                #endregion

                //进行网络消息解析
                NetworkMessageAdapter(handlerContext);

                NetworkPackageMutex.ReleaseMutex();
            }
            else//如果网络消息阻塞时间达到2秒以上
            {
                CacheNetworkMessage = null;
            }
        }

        /// <summary>
        /// 网络消息适配器
        /// </summary>
        private void NetworkMessageAdapter(IChannelHandlerContext handlerContext)
        {
            if (CacheNetworkMessage.Length >= 14)
            {
                if (CacheNetworkMessage[0] == NetworkDeploy.HeadByte1 && CacheNetworkMessage[1] == NetworkDeploy.HeadByte2)//帧头匹配
                {
                    int packageLength = BitConverter.ToUInt16(CacheNetworkMessage, 6);
                    if (CacheNetworkMessage.Length >= packageLength)//已经接收到足够长的消息 进入解析
                    {
                        byte[] analysisByte = new byte[packageLength];
                        Array.Copy(CacheNetworkMessage, 0, analysisByte, 0, packageLength); //进行拼包处理

                        if (analysisByte[analysisByte.Length - 4] == NetworkDeploy.EndByte1 && analysisByte[analysisByte.Length - 3] == NetworkDeploy.EndByte2)
                        {
                            byte[] _CRCCheckBytes = new byte[analysisByte.Length - 2];
                            Array.Copy(analysisByte, _CRCCheckBytes, _CRCCheckBytes.Length);
                            if (analysisByte[analysisByte.Length - 2] == CRCHelper.CRCL(_CRCCheckBytes) &&
                                analysisByte[analysisByte.Length - 1] == CRCHelper.CRCH(_CRCCheckBytes))//校验和通过 输出参数
                            {
                                //合法输出参数
                                ProtocolHandleOverEvent?.Invoke(handlerContext, analysisByte);
                            }
                        }
                        #region 进行消息裁切
                        byte[] cuttingCacheNetworkMessage = new byte[CacheNetworkMessage.Length - packageLength];
                        if (cuttingCacheNetworkMessage.Length > 0)
                            Array.Copy(CacheNetworkMessage, packageLength, cuttingCacheNetworkMessage, 0, cuttingCacheNetworkMessage.Length);
                        CacheNetworkMessage = cuttingCacheNetworkMessage;
                        #endregion
                        //继续解析
                        NetworkMessageAdapter(handlerContext);
                    }
                }
                else//帧头不匹配时丢弃所有非法消息
                {
                    CacheNetworkMessage = null;
                }
            }
        }
    }
}
