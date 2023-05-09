using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Struct
{
    /// <summary>
    /// 各个模块之间通信消息头
    /// </summary>
    public struct StandardHeadForClient
    {
        #region 通用消息 例:模型操作 设置模型
        /// <summary>
        /// 空消息
        /// </summary>
        public const byte Empty = 0x00;

        /// <summary>
        /// 文本消息
        /// </summary>
        public const byte TextMeaasge = 0x01;

        /// <summary>
        /// 对象消息发送
        /// </summary>
        public const byte ObjectMessage = 0x02;
        #endregion
    }
}
