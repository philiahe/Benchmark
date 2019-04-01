using Coldairarrow.DotNettyRPC;
using Common;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            RPCServer rPCServer = new RPCServer(CommonHelper.DotNettyPort);
            rPCServer.RegisterService<IHello, Hello>();
            rPCServer.Start();

            Console.WriteLine($"RpcServer started on {CommonHelper.DotNettyPort}");
            Console.ReadLine();
        }
    }
}
