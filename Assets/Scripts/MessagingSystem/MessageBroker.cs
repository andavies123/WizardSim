using System.Collections.Generic;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace MessagingSystem
{
	public static class MessageBroker
	{
		private static readonly SubscriptionStore SubscriptionStore = new();
		private static readonly MessageStore MessageStore = new();

		public static void Subscribe(ISubscription subscription)
		{
			if (!SubscriptionStore.TryAddSubscription(subscription))
			{
				Debug.LogWarning($"Subscription from {subscription.Subscriber} failed for message type: {subscription.MessageType}");
			}
		}

		public static void Unsubscribe(ISubscription subscription)
		{
			SubscriptionStore.DeleteSubscription(subscription);
		}

		public static void PublishSingle<TMessage>(TMessage message) where TMessage : IMessage
		{
			if (message == null) // Invalid message
			{
				Debug.LogWarning($"Unable to publish null {typeof(TMessage)}");
				return;
			}

			if (SubscriptionStore.TryGetSubscriptions<TMessage>(out List<ISubscription> subscriptions))
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
		
		public static void PublishPersistant<TKey, TMessage>(TKey key, TMessage message)
			where TKey : IMessageKey where TMessage : IMessage
		{
			// Add to the message store
			MessageStore.AddMessage(key, message);
			
			// Same as publishing to everyone right now
			PublishSingle(message);
		}

		public static void DeletePersistant<TKey>(TKey key) where TKey : IMessageKey
		{
			if (key == null)
				return;

			MessageStore.TryDeleteMessage(key);
		}
	}
}