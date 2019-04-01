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
            var ip = CommonHelper.GetLocalIp();
            string urls = $"http://{ip}:{CommonHelper.WebApiPort}";

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
