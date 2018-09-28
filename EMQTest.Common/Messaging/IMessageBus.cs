using EMQTest.Common.Messaging.Commands;
using EMQTest.Common.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMQTest.Common.Messaging
{
    public interface IMessageBus
    {
        Task PublishEvent(IEvent @event);

        Task<TResponse> Request<TRequest, TResponse>(TRequest request) where TRequest : ICommand where TResponse : ICommandResponse;
    }
}
