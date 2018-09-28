using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMQTest.Common.Messaging.Events
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
