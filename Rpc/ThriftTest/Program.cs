using System;
using Thrift.Server;
using Thrift.Transport;

namespace ThriftTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TServerSocket serverTransport = new TServerSocket(32040, 0, false);
            Helloword.Processor processor = new Helloword.Processor(new HellowordThrift());
            TServer server = new TThreadPoolServer(processor, serverTransport);
            Console.WriteLine($"RpcServer started on {32040}");
            server.Serve();
            Console.ReadKey();
        }

        public class HellowordThrift : Helloword.Iface
        {
            public SayHelloResultArgs SayHello(SayHelloArgs args)
            {
                return new SayHelloResultArgs { Message = "Hello " + args.Name };
            }
        }
    }
}
