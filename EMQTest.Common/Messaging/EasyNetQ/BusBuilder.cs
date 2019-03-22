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

        private List<(Type EventType, Type EventHandlerType, Type EventHandlerImplementationType)> _events = new List<(Type Event, Type EventHandlerType, Type EventHandlerImplementation)>();
        
        private List<KeyValuePair<(Type Command, Type CommandResponse), Type>> _commands2 = new List<KeyValuePair<(Type Command, Type CommandResponse), Type>>();

        public BusBuilder(IWebHostBuilder webHostBuilder, string connectionString)
        {
            _webHostBuilder = webHostBuilder;
            _connectionString = connectionString;
        }

        public BusBuilder AddEventHandler<TEvent, TEventHandler>() 
            where TEvent : IEvent 
            where TEventHandler : IEventHandler<TEvent>, IEventHandlerDescriptor<TEvent, IEventHandler<TEvent>>
        {
            _events.Add((typeof(TEvent),typeof(IEventHandler<TEvent>), typeof(TEventHandler)));

            return this;
        }

        public BusBuilder AddCommandHandler<TCommand, TCommandResponse, TCommandHandler>()
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
            where TCommandHandler : ICommandHandler<TCommand, TCommandResponse>
        {
            _commands2.Add(new KeyValuePair<(Type Command, Type CommandResponse), Type>((typeof(TCommand), typeof(TCommandResponse)), typeof(TCommandHandler)));

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
                        serviceCollection.AddTransient(typeof(IEventHandlerDescriptor<,>).MakeGenericType(@event.EventType, @event.EventHandlerType), @event.EventHandlerImplementationType);
                    }

                    foreach (var command in _commands2)
                    {
                        serviceCollection.AddTransient(typeof(ICommandHandler<,>).MakeGenericType(command.Key.Command, command.Key.CommandResponse), command.Value);
                    }
                })
                .Build();

            var busClient = host.Services.GetRequiredService<IBus>();

            var addEventHandlerMethodInfo = typeof(BusExtension).GetMethod(nameof(BusExtension.AddEventHandler));

            foreach (var @event in _events)
            {
                var generic = addEventHandlerMethodInfo.MakeGenericMethod(@event.EventType, @event.EventHandlerType, @event.EventHandlerImplementationType);

                generic.Invoke(busClient, new object[] { busClient, host.Services });
            }

            var addCommandHandlerMethodInfo = typeof(BusExtension).GetMethod(nameof(BusExtension.AddCommandHandler));

            foreach (var command in _commands2)
            {
                var generic = addCommandHandlerMethodInfo.MakeGenericMethod(command.Key.Command, command.Key.CommandResponse);

                generic.Invoke(busClient, new object[] { busClient, host.Services });
            }

            return host;
        }
    }
}
