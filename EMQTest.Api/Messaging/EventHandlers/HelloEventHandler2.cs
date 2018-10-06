using EMQTest.Common.Messaging.Events;
using EMQTest.Messages.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMQTest.Api.Messaging.EventHandlers
{
    public class HelloEventHandler2 : IEventHandler<HelloEvent>
    {
        private readonly ILogger<HelloEventHandler2> _logger;

        public HelloEventHandler2(ILogger<HelloEventHandler2> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(HelloEvent @event)
        {
            _logger.LogInformation(@event.EventId.ToString());

            return Task.CompletedTask;
        }
    }
}
