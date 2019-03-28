using Common;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace GrpcTest
{
    class Program
    {
        const int Port = 32000;

        static void Main(string[] args)
        {
            string ip = NetworkHelper.GetLocalIp();

            Server server = new Server
            {
                Services = { Helloworld.BindService(new HelloImpl()) },
                Ports = { new ServerPort(ip, Port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine($"RpcServer started on {ip}:{Port}");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }

    class HelloImpl : Helloworld.HelloworldBase
    {
        public override Task<SayHelloResultArgs> SayHello(SayHelloArgs args, ServerCallContext context)
        {
            Console.WriteLine(args.Name);

            return Task.FromResult(new SayHelloResultArgs { Message = "Hello " + args.Name });
        }
    }
}
