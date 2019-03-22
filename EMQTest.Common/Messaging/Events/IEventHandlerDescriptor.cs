namespace EMQTest.Common.Messaging.Events
{
    public interface IEventHandlerDescriptor<TEvent, TImplementation> where TEvent : IEvent where TImplementation : IEventHandler<TEvent>
    {
    }
}
