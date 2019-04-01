using Common;
using System;

namespace DotNettyTest
{
    public class Hello : IHello
    {
        static int Count = 0;

        public SayHelloResultArgs SayHello(SayHelloArgs args)
        {
            //Console.WriteLine(args.Name + Count++);

            return new SayHelloResultArgs { Message = "Hello " + args.Name };
        }
    }
}
