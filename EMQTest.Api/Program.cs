using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EMQTest.Common.Messaging.EasyNetQ.Extensions;
using EMQTest.Messages.Events;
using EMQTest.Api.Messaging.EventHandlers;
using EMQTest.Messages.Commands;
using EMQTest.Api.Messaging.CommandHandlers;

namespace EMQTest.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("hosting.json", optional: true)
                   .AddJsonFile("appsettings.json", optional: false)
                   .AddEnvironmentVariables()
                   .AddCommandLine(args)
                   .Build();

            CreateWebHostBuilder(args)
                .UseConfiguration(configuration)
                .UseEasyNetQ("host=localhost")
                    .AddEventHandler<HelloEvent, HelloEventHandler>()
                    .AddCommandHandler<HelloCommand, HelloCommandResponse, HelloCommandHandler>()
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
