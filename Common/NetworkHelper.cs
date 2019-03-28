using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    public class NetworkHelper
    {
        /// <summary>
        /// 获取当前计算机的IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            string result = "0.0.0.0";

            string host = Dns.GetHostName();
            var addrs = Dns.GetHostAddresses(host);
            foreach (var item in addrs)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    result = item.ToString();
                }
            }

            return result;
        }
    }
}
