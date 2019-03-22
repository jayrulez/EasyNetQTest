using EasyNetQ;
using EMQTest.Common.Messaging.Commands;
using EMQTest.Common.Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMQTest.Common.Messaging.EasyNetQ.Extensions
{
    public static class BusExtension
    {
        public static void AddEventHandler<TEvent, TEventHandlerType, TImplementationType>(this IBus busClient, IServiceProvider serviceProvider)
            where TEvent : class, IEvent
            where TEventHandlerType : IEventHandler<TEvent>
            where TImplementationType : IEventHandler<TEvent>
        {
            var eventHandlerServices = serviceProvider.GetServices<IEventHandlerDescriptor<TEvent, TEventHandlerType>>();

            var eventHandler = eventHandlerServices.FirstOrDefault(service => service.GetType() == typeof(TImplementationType)) as IEventHandler<TEvent>;

            if (eventHandler != null)
            {
                busClient.SubscribeAsync<TEvent>(nameof(TEvent), async (@event) =>
                {
                    await eventHandler.HandleAsync(@event);
                });
            }
            else
            {
                throw new Exception($"No event handler was found for event type '{typeof(TEvent).Name}'.");
            }
        }

        public static void AddCommandHandler<TCommand, TCommandResponse>(this IBus busClient, IServiceProvider serviceProvider)
            where TCommand : class, ICommand
            where TCommandResponse : class, ICommandResponse
        {

            busClient.RespondAsync<TCommand, TCommandResponse>(async (command) =>
            {
                var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResponse>>();

                var response = await handler.HandleAsync(command);

                return response;
            });
        }
    }
}
