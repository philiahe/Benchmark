using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// 扩展UseUrls()，支持以命令方式给端口赋值并启动（eg. dotnet --port 3222）
        /// </summary>
        /// <returns>.</returns>
        public static IWebHostBuilder UseUrlPort(this IWebHostBuilder hostBuilder, string[] args)
        {
            //
            // 默认以本机IP:5010端口启动（使用Consul做服务注册，IP无法指定为*）
            var ip = NetworkHelper.GetLocalIp();
            string urls = $"http://{ip}:5010";

            if (args != null)
            {
                var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
                if (configuration["port"] != null)
                    urls = $"http://{ip}:{configuration["port"]}";
            }

            return hostBuilder.UseUrls(urls);
        }
    }
}
