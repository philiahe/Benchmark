using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using HelloWorld.Grains;
using Orleans;
using Orleans.Hosting;

namespace OrleansSiloTest
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            Console.Title = nameof(OrleansSiloTest);

            return new HostBuilder()
               .UseOrleans(builder =>
               {
                   builder
                       .UseLocalhostClustering()
                       .ConfigureApplicationParts(manager =>
                       {
                           manager.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences();
                       });
               })
               .ConfigureLogging(builder =>
               {
                   builder.AddConsole();
               })
               .RunConsoleAsync();
        }
    }
}
