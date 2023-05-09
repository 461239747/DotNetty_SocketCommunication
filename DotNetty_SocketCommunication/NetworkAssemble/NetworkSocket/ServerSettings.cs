using System.Collections.Generic;

using DotNetty.Transport.Channels;

namespace NetworkSocket
{
    public static class ServerSettings
    {
        /// <summary>
        /// 决定是否使用 Libuv 作为底层传输实现
        /// Windows中一般使用True即可
        /// </summary>
        public static bool UseLibuv
        {
            get
            {
                return true;
            }
        }
    }
}
