using EMQTest.Common.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMQTest.Messages.Events
{
    public class HelloEvent : IEvent
    {
        public Guid EventId { get; set; }
    }
}
