using Common;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace GrpcTest
{
    class Program
    {
        

        static void Main(string[] args)
        {
            string ip = CommonHelper.GetLocalIp();

            Server server = new Server
            {
                Services = { Helloworld.BindService(new HelloImpl()) },
                Ports = { new ServerPort(ip, CommonHelper.GrpcPort, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine($"RpcServer started on {ip}:{CommonHelper.GrpcPort}");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }

    class HelloImpl : Helloworld.HelloworldBase
    {
        static int Count = 0;
        public override Task<SayHelloResultArgs> SayHello(SayHelloArgs args, ServerCallContext context)
        {
            Console.WriteLine(args.Name + Count++);

            return Task.FromResult(new SayHelloResultArgs { Message = $"Hello {args.Name}" });
        }
    }
}
