using Coldairarrow.DotNettyRPC;
using Common;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNettyTest;
using Grpc.Core;
using GrpcTest;
using HelloWorld.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;

namespace Test
{
    class Program
    {
        static string ServerIp = "";
        static int Times;
        static int ThreadCount;
        static Common.SayHelloArgs CommonArgs = new Common.SayHelloArgs { Name = "philia" };

        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();
            Times = Convert.ToInt32(configuration["Times"]);
            ThreadCount = Convert.ToInt32(configuration["ThreadCount"]);

            //Grpc();

            //Orleans();

            //DotNettyTest();

            Thrift();

            //HttpClient();
            //HttpWebRequest();

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
            var args = new GrpcTest.SayHelloArgs { Name = "philia" };

            CodeTimerPro.Start("Grpc", Times, p =>
            {
                var result = client.SayHello(args);
            }, ThreadCount);

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
            using (var client = await OrleansClient())
            {
                var friend = client.GetGrain<HelloWorld.Interfaces.IHello>(0);
                CodeTimerPro.Start("Orleans", Times, async _ =>
                {
                    var result = await friend.SayHello(CommonArgs);
                        //Console.WriteLine(result.Message);
                }, ThreadCount);

                Console.ReadKey();
            }

            return 1;
        }

        private static async Task<IClusterClient> OrleansClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => { options.AdvertisedIPAddress = IPAddress.Parse(LocalIp); })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            return client;
        }

        #endregion

        #region -- DotNetty --

        static void DotNettyTest()
        {
            DotNettyTest.IHello client = RPCClientFactory.GetClient<DotNettyTest.IHello>("127.0.0.1", 32030);
            CodeTimerPro.Start("DotNetty", Times, _ =>
            {
                client.SayHello("philia");
            }, ThreadCount);
        }

        #endregion

        #region -- Thrift --
        static void Thrift()
        {
            CodeTimerPro.Start("thrift", Times, _ =>
            {
                TTransport transport = new TSocket("127.0.0.1", 32040);
                transport.Open();
                TProtocol protocol = new TBinaryProtocol(transport);
                using (Helloword.Client client = new Helloword.Client(protocol))
                {
                    var reply = client.SayHello(new SayHelloArgs { Name = "philia" });
                }
                transport.Close();
            }, ThreadCount);
        }

        #endregion

        #region -- WebApi --

        static void HttpClient()
        {
            var client = new HttpClient();
            var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(CommonArgs));
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            CodeTimerPro.Start("HttpClient", Times, _ =>
            {
                var response =  client.PostAsync($"http://{LocalIp}:5010/api/Values", byteContent);
                var result = response.Result.Content.ReadAsStringAsync().Result;
            }, ThreadCount);
        }

        static void HttpWebRequest()
        {
            string url = $"http://{LocalIp}:5010/api/Values";
           
            CodeTimerPro.Start("HttpWebRequest", Times, _ =>
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webRequest.Method = "post";
                webRequest.ContentType = "application/application/json";
                string postData = "Name=philia";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                webRequest.ContentLength = byteArray.Length;
                System.IO.Stream newStream = webRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                string result = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
            }, ThreadCount);
        }

        #endregion

    }
}
