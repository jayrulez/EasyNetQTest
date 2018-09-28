using EasyNetQ;
using EMQTest.Common.Messaging.Commands;
using EMQTest.Common.Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMQTest.Common.Messaging.EasyNetQ.Extensions
{
    public static class BusExtension
    {
        public static void AddEventHandler<TEvent>(this IBus busClient, IServiceProvider serviceProvider)
            where TEvent : class, IEvent
        {
            busClient.SubscribeAsync<TEvent>(nameof(TEvent), async (@event) =>
            {
                var handler = serviceProvider.GetRequiredService<IEventHandler<TEvent>>();

                await handler.HandleAsync(@event);
            });
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
