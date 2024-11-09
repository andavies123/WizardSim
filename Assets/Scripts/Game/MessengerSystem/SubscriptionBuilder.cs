using System;
using UnityEngine;

namespace Game.MessengerSystem
{
	public class SubscriptionBuilder
	{
		private readonly object _subscriber;
		private Type _messageType;
		private Action<IMessage> _callback;
		private Predicate<IMessage> _messageFilter;
		
		public SubscriptionBuilder(object subscriber)
		{
			_subscriber = subscriber;
		}

		public SubscriptionBuilder SetMessageType<TMessage>() where TMessage : IMessage
		{
			_messageType = typeof(TMessage);
			return this;
		}

		public SubscriptionBuilder SetCallback(Action<IMessage> callback)
		{
			_callback = callback;
			return this;
		}

		public SubscriptionBuilder AddFilter(Predicate<IMessage> filter)
		{
			_messageFilter = filter;
			return this;
		}

		public SubscriptionBuilder ResetAllButSubscriber()
		{
			_messageType = null;
			_callback = null;
			_messageFilter = null;
			return this;
		}

		public ISubscription Build()
		{
			if (!IsComplete())
				Debug.LogWarning("Building an incomplete subscription object");
			
			return new Subscription(_subscriber, _messageType, _messageFilter, _callback);
		}

		private bool IsComplete()
		{
			return _subscriber != null && _messageType != null && _callback != null;
		}
	}
}