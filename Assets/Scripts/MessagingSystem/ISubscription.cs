using System;

namespace MessagingSystem
{
	public struct Subscription : ISubscription
	{
		public Subscription(object subscriber, Type messageType, Predicate<IMessage> messageFilter, Action<IMessage> callback)
		{
			Subscriber = subscriber;
			MessageType = messageType;
			MessageFilter = messageFilter;
			Callback = callback;
		}

		public object Subscriber { get; }
		public Type MessageType { get; }
		public Predicate<IMessage> MessageFilter { get; }
		public Action<IMessage> Callback { get; }
	}

	public interface ISubscription
	{
		/// <summary>
		/// The name of the owner of the subscription
		/// </summary>
		object Subscriber { get; }
		
		/// <summary>
		/// The type of message that will be subscribed to
		/// </summary>
		Type MessageType { get; }
		
		/// <summary>
		/// Filter validation so the subscriber does not receive unnecessary messages
		/// </summary>
		Predicate<IMessage> MessageFilter { get; }
		
		/// <summary>
		/// The callback for when a valid message is published
		/// </summary>
		Action<IMessage> Callback { get; }
	}
}