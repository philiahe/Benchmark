using HelloWorld.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Common;

namespace HelloWorld.Grains
{
    /// <summary>
    /// Orleans grain implementation class HelloGrain.
    /// </summary>
    public class HelloGrain : Orleans.Grain, IHello
    {
        static int Count = 0;
        private readonly ILogger logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            this.logger = logger;
        }  

        Task<SayHelloResultArgs> IHello.SayHello(SayHelloArgs args)
        {
            //logger.LogInformation($"SayHello message received: greeting = '{args}'");
            //logger.LogInformation(args.Name + Count++);

            return Task.FromResult(new SayHelloResultArgs { Message = $"Hello {args.Name}" });
        }
    }
}
