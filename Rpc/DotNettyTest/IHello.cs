using Common;

namespace DotNettyTest
{
    public interface IHello
    {
        SayHelloResultArgs SayHello(SayHelloArgs msg);
    }
}
