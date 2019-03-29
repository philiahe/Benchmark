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
        private readonly ILogger logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            this.logger = logger;
        }  

        Task<SayHelloResultArgs> IHello.SayHello(SayHelloArgs args)
        {
            logger.LogInformation($"SayHello message received: greeting = '{args}'");

            return Task.FromResult(new SayHelloResultArgs { Message = $"Hello {args.Name}" });
        }
    }
}
