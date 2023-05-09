using System.Collections.Concurrent;
using System.Linq;

using DotNetty.Transport.Channels;

using Network.Struct;

using NetworkHelper;

namespace DotNetty_SocketCommunication
{
    /// <summary>
    /// 客户端连接信息
    /// </summary>
    public class ExternalLoginClient
    {
        /// <summary>
        /// 外接各模块客户端信息键值对
        /// [键:用户ID 值:数据管道]
        /// </summary>
        public static ConcurrentDictionary<int, IChannelHandlerContext> ExternalClientDic
        {
            get
            {
                return _ExternalClientDic;
            }
            set
            {
                _ExternalClientDic = value;
            }
        }
        private static ConcurrentDictionary<int, IChannelHandlerContext> _ExternalClientDic;

        /// <summary>
        /// 子模块登录
        /// </summary>
        /// <param name=" _StudentID"></param>
        /// <param name="_LoginClient"></param>
        /// <returns></returns>
        public static bool ExternalLogin(int userID, IChannelHandlerContext _LoginClient)
        {
            if (_ExternalClientDic == null)//多个线程访问键值对  键值对=null
                _ExternalClientDic = new ConcurrentDictionary<int, IChannelHandlerContext>();//创建键值对

            if (!_ExternalClientDic.ContainsKey(userID))//如果不包含值
            {
                bool _IsAdd = _ExternalClientDic.TryAdd(userID, _LoginClient);//添加键值对
                return _IsAdd;
            }
            else if (_ExternalClientDic[userID].Channel != _LoginClient.Channel)
            {
                _ExternalClientDic[userID].CloseAsync();//断开

                _ExternalClientDic[userID] = _LoginClient;
                return true;
            }
            return true;
        }

        /// <summary>
        /// 客户端退出登录
        /// </summary>
        /// <param name="_ExtendSocket"></param>
        /// <returns></returns>
        public static bool ExternalExtend(IChannelHandlerContext _ExtendSocket)
        {
            if (ExternalClientDic != null)
            {
                int _ClientType = ExternalClientDic.FirstOrDefault(e => e.Value.Channel == _ExtendSocket.Channel).Key;
                IChannelHandlerContext _RemoveSocket;
                bool _IsTrue = ExternalClientDic.TryRemove(_ClientType, out _RemoveSocket);

                return _IsTrue;
            }
            else
                return false;
        }

        /// <summary>
        /// 客户端退出登录
        /// </summary>
        /// <param name="_ExtendSocket"></param>
        /// <param name="externalStudentID">退出登录的主键ID</param>
        /// <returns></returns>
        public static bool ExternalExtend(IChannelHandlerContext _ExtendSocket, out int externalStudentID)
        {
            if (ExternalClientDic != null)
            {
                externalStudentID = ExternalClientDic.FirstOrDefault(e => e.Value.Channel == _ExtendSocket.Channel).Key;
                IChannelHandlerContext _RemoveSocket;
                bool _IsTrue = ExternalClientDic.TryRemove(externalStudentID, out _RemoveSocket);

                return _IsTrue;
            }
            else
            {
                externalStudentID = 0;
                return false;
            }
        }
    }
}
