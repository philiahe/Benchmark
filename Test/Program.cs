using Common;
using Grpc.Core;
using GrpcTest;
using Microsoft.Extensions.Configuration;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();
            var times = Convert.ToInt32(configuration["Times"]); 
            int threadCount = Convert.ToInt32(configuration["ThreadCount"]);

            string ip = NetworkHelper.GetLocalIp();
            Channel channel = new Channel($"{ip}:32000", ChannelCredentials.Insecure);
            CodeTimerPro.Start("gRpc", times, p =>
            {
                var client = new Helloworld.HelloworldClient(channel);
                var result = client.SayHello(new GrpcTest.SayHelloArgs { Name = "philia" + p });
            }, threadCount);

            channel.ShutdownAsync().Wait();

            Console.ReadKey();
        }
    }
}
