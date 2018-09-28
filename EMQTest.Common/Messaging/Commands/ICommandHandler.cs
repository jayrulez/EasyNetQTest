using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMQTest.Common.Messaging.Commands
{
    public interface ICommandHandler<TCommand, TCommandResponse> where TCommand : ICommand where TCommandResponse : ICommandResponse
    {
        Task<TCommandResponse> HandleAsync(TCommand command);
    }
}
