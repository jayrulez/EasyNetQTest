using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMQTest.Common.Messaging.Events
{
    public interface IEventHandlerDescriptor<TEvent, TImplementation> where TEvent : IEvent where TImplementation : IEventHandler<TEvent>
    {
    }
}
