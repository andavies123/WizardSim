using System.Collections.Generic;
using UnityEngine;

namespace MessagingSystem
{
	public class MessageBroker : IMessageBroker
	{
		private readonly ISubscriptionStore _subscriptionStore;
		private readonly IMessageStore _messageStore;

		public MessageBroker()
		{
			_subscriptionStore = new SubscriptionStore();
			_messageStore = new MessageStore();
		}
		
		internal MessageBroker(ISubscriptionStore subscriptionStore, IMessageStore messageStore)
		{
			_subscriptionStore = subscriptionStore;
			_messageStore = messageStore;
		}

		public void Subscribe(ISubscription subscription)
		{
			if (!_subscriptionStore.TryAddSubscription(subscription))
			{
				Debug.LogWarning($"Subscription from {subscription.Subscriber} failed for message type: {subscription.MessageType}");
			}
		}

		public void Unsubscribe(ISubscription subscription)
		{
			_subscriptionStore.DeleteSubscription(subscription);
		}

		public void PublishSingle<TMessage>(TMessage message) where TMessage : IMessage
		{
			if (message == null) // Invalid message
			{
				Debug.LogWarning($"Unable to publish null {typeof(TMessage)}");
				return;
			}

			if (_subscriptionStore.TryGetSubscriptions<TMessage>(out List<ISubscription> subscriptions))
			{
				foreach (ISubscription subscription in subscriptions)
				{
					if (subscription.MessageFilter?.Invoke(message) ?? true) // Null filter counts as no filter
					{
						subscription.Callback?.Invoke(message);
					}
				}
			}
		}
		
		public void PublishPersistant<TKey, TMessage>(TKey key, TMessage message)
			where TKey : IMessageKey where TMessage : IMessage
		{
			if (key == null || message == null)
			{
				Debug.LogWarning($"Unable to publish persistant message with a null key: {typeof(TKey)} or message: {typeof(TMessage)}");
				return;
			}
			
			// Add to the message store
			_messageStore.AddMessage(key, message);
			
			// Same as publishing to everyone right now
			PublishSingle(message);
		}

		public bool TryGetPersistantMessage(IMessageKey key, out IMessage message)
		{
			return _messageStore.TryGetMessage(key, out message);
		}

		public void DeletePersistant<TKey>(TKey key) where TKey : IMessageKey
		{
			if (key == null)
				return;

			_messageStore.TryDeleteMessage(key);
		}
	}
}