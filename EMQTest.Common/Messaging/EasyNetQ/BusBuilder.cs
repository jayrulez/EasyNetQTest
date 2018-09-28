using EasyNetQ;
using EMQTest.Common.Messaging.Commands;
using EMQTest.Common.Messaging.EasyNetQ.Extensions;
using EMQTest.Common.Messaging.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EMQTest.Common.Messaging.EasyNetQ
{
    public class BusBuilder
    {
        private readonly IWebHostBuilder _webHostBuilder;
        private readonly string _connectionString;

        private Dictionary<Type, Type> _events = new Dictionary<Type, Type>();
        private Dictionary<(Type Command, Type CommandResponse), Type> _commands = new Dictionary<(Type Command, Type CommandResponse), Type>();

        public BusBuilder(IWebHostBuilder webHostBuilder, string connectionString)
        {
            _webHostBuilder = webHostBuilder;
            _connectionString = connectionString;
        }

        public BusBuilder AddEventHandler<TEvent, TEventHandler>() where TEvent : IEvent where TEventHandler : IEventHandler<TEvent>
        {
            _events.Add(typeof(TEvent), typeof(TEventHandler));

            return this;
        }

        public BusBuilder AddCommandHandler<TCommand, TCommandResponse, TCommandHandler>()
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
            where TCommandHandler : ICommandHandler<TCommand, TCommandResponse>
        {
            _commands.Add((typeof(TCommand), typeof(TCommandResponse)), typeof(TCommandHandler));

            return this;
        }

        public IWebHost Build()
        {
            var host = _webHostBuilder
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.RegisterEasyNetQ(_connectionString);

                    foreach (var @event in _events)
                    {
                        serviceCollection.AddTransient(typeof(IEventHandler<>).MakeGenericType(@event.Key), @event.Value);
                    }

                    foreach (var command in _commands)
                    {
                        serviceCollection.AddTransient(typeof(ICommandHandler<,>).MakeGenericType(command.Key.Command, command.Key.CommandResponse), command.Value);
                    }
                })
                .Build();

            var busClient = host.Services.GetRequiredService<IBus>();


            foreach (var @event in _events)
            {
                var methodInfo = typeof(BusExtension).GetMethod(nameof(BusExtension.AddEventHandler));

                var generic = methodInfo.MakeGenericMethod(@event.Key);

                generic.Invoke(busClient, new object[] { busClient, host.Services });
            }

            foreach (var command in _commands)
            {
                var methodInfo = typeof(BusExtension).GetMethod(nameof(BusExtension.AddCommandHandler));

                var generic = methodInfo.MakeGenericMethod(command.Key.Command, command.Key.CommandResponse);

                generic.Invoke(busClient, new object[] { busClient, host.Services });
            }

            return host;
        }
    }
}
