using Common;
using Grpc.Core;
using GrpcTest;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = NetworkHelper.GetLocalIp();

            Channel channel = new Channel($"{ip}:32000", ChannelCredentials.Insecure);
            CodeTimerPro.Start("gRpc", 1000, p =>
            {
                var client = new Helloworld.HelloworldClient(channel);
                var result = client.SayHello(new GrpcTest.SayHelloArgs { Name = "philia" + p });
            }, 10);

            channel.ShutdownAsync().Wait();

            Console.ReadKey();
        }
    }
}
