using Common;
using System;
using Thrift.Server;
using Thrift.Transport;

namespace ThriftTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TServerSocket serverTransport = new TServerSocket(CommonHelper.ThriftPort, 0, false);
            Helloword.Processor processor = new Helloword.Processor(new HellowordThrift());
            TServer server = new TThreadPoolServer(processor, serverTransport);
            Console.WriteLine($"RpcServer started on {CommonHelper.ThriftPort}");
            server.Serve();
            Console.ReadKey();
        }

        public class HellowordThrift : Helloword.Iface
        {
            static int Count = 0;
            public SayHelloResultArgs SayHello(SayHelloArgs args)
            {
                //Console.WriteLine(args.Name + Count++);
                return new SayHelloResultArgs { Message = "Hello " + args.Name };
            }
        }
    }
}
