using EMQTest.Common.Messaging.Commands;
using EMQTest.Messages.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMQTest.Api.Messaging.CommandHandlers
{
    public class HelloCommandHandler : ICommandHandler<HelloCommand, HelloCommandResponse>
    {
        private readonly ILogger _logger;

        public HelloCommandHandler(ILogger<HelloCommandHandler> logger)
        {
            _logger = logger;

        }

        public Task<HelloCommandResponse> HandleAsync(HelloCommand command)
        {
            //_logger.LogInformation($"HelloCommand handled from Source: '{messageContext.Source}'. ExecutionId='{messageContext.ExecutionId}'.");

            var response = new HelloCommandResponse
            {
                ResponseId = command.CommandId
            };

            return Task.FromResult(response);
        }
    }
}
