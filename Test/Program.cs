using Common;
using Grpc.Core;
using GrpcTest;
using HelloWorld.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static string LocalIp;
        static int Times;
        static int threadCount;

        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();
            Times = Convert.ToInt32(configuration["Times"]);
            threadCount = Convert.ToInt32(configuration["ThreadCount"]);
            LocalIp = NetworkHelper.GetLocalIp();

            //Grpc();

            Orleans();

            Console.ReadKey();
        }

        #region -- Grpc --
        /// <summary>
        /// Grpc
        /// </summary>
        /// <remark>
        /// 1、Google
        /// 2、跨平台（原生）
        /// 3、基于Http2设计，支持双向流、消息头压缩、单 TCP 的多路复用、服务端推送等特性，在移动端设备上更加省电和节省网络流量
        /// 4、VS2019原生支持，不需要脚本生成代理类
        /// </remark>
        static void Grpc()
        {
            Channel channel = new Channel($"{LocalIp}:32000", ChannelCredentials.Insecure);
            var client = new Helloworld.HelloworldClient(channel);

            CodeTimerPro.Start("gRpc", Times, p =>
            {
                var result = client.SayHello(new GrpcTest.SayHelloArgs { Name = "philia" + p });
            }, threadCount);

            channel.ShutdownAsync().Wait();
        }
        #endregion

        #region -- Orleans --

        /// <summary>
        /// Orleans
        /// </summary>
        /// <remark>
        /// 1、微软
        /// 2、分布式计算框架，不止Rpc，内部还包含了服务发现、负载均衡、高可用等处理机制（Consul）
        /// </remark>
        static void Orleans()
        {
            var result = RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await StartClientWithRetries())
                {
                    var friend = client.GetGrain<IHello>(0);
                    CodeTimerPro.Start("Orleans", Times, async p =>
                    {
                        var result = await friend.SayHello(new Common.SayHelloArgs { Name = "philia" + p });
                        //Console.WriteLine(result.Message);
                    }, threadCount);

                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> StartClientWithRetries()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => { options.AdvertisedIPAddress = IPAddress.Parse(NetworkHelper.GetLocalIp()); })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            return client;
        }

        #endregion


    }
}
