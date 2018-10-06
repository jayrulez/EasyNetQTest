using System.Threading.Tasks;

namespace EMQTest.Common.Messaging.Events
{
    public interface IEventHandler<TEvent> : IEventHandlerDescriptor<TEvent, IEventHandler<TEvent>> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
