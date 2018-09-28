using EMQTest.Common.Messaging.Events;
using EMQTest.Messages.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMQTest.Api.Messaging.EventHandlers
{
    public class HelloEventHandler : IEventHandler<HelloEvent>
    {
        private readonly ILogger _logger;

        public HelloEventHandler(ILogger<HelloEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(HelloEvent @event)
        {
            //_logger.LogInformation($"HelloEvent handled from Source: '{messageContext.Source}'. ExecutionId='{messageContext.ExecutionId}'");

            _logger.LogInformation(@event.EventId.ToString());

            return Task.CompletedTask;
        }
    }
}
