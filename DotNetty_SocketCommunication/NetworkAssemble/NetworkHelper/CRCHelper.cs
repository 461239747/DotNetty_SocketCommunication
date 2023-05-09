using System;
using System.Linq;
using System.Text;

namespace NetworkHelper
{
    /// <summary>
    /// CRC校验帮助类
    /// 定义:南国灿
    /// 郑伟 2022-03-30
    /// </summary>
    public class CRCHelper
    {
        #region CRC16
        /// <summary>
        /// CRC高位校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte CRCH(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8); //高位置
                return hi;
            }
            return 0x00;
        }

        /// <summary>
        /// CRC低位校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte CRCL(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte lo = (byte)(crc & 0x00FF); //低位置

                return lo;
            }
            return 0x00;
        }

        #endregion
    }
}
