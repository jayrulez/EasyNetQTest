﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EMQTest.Api.Messaging.EventHandlers;
using EMQTest.Common.Messaging.Events;
using EMQTest.Messages.Commands;
using EMQTest.Messages.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EMQTest.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IEventHandlerDescriptor<HelloEvent, IEventHandler<HelloEvent>>, HelloEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var busClient = app.ApplicationServices.GetRequiredService<IBus>();

                var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();

                var response = await busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand
                {
                    CommandId = Guid.NewGuid()
                });

                logger.LogInformation($"Response: {JsonConvert.SerializeObject(response)}");

                await busClient.PublishAsync(new HelloEvent
                {
                    EventId = Guid.NewGuid()
                });

                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
