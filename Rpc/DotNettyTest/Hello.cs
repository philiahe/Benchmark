using System;

namespace DotNettyTest
{
    public class Hello : IHello
    {
        public void SayHello(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
