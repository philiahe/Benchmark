using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using HelloWorld.Grains;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace OrleansSiloHost
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            Console.Title = nameof(OrleansSiloHost);

            return new HostBuilder()
               .UseOrleans(builder =>
               {
                   builder
                       .UseLocalhostClustering()
                       .ConfigureApplicationParts(manager =>
                       {
                           manager.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences();
                       })
                       .Configure<EndpointOptions>(options => {  });
               })
               .ConfigureLogging(builder =>
               {
                   builder.AddConsole();
               })
               .RunConsoleAsync();
        }
    }
}
