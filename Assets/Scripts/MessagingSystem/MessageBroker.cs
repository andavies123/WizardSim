﻿using System.Collections.Generic;
using UnityEngine;

namespace MessagingSystem
{
	public class MessageBroker : IMessageBroker
	{
		internal ISubscriptionStore SubscriptionStore { get; }
		internal IMessageStore MessageStore { get; }

		public MessageBroker()
		{
			SubscriptionStore = new SubscriptionStore();
			MessageStore = new MessageStore();
		}
		
		internal MessageBroker(ISubscriptionStore subscriptionStore, IMessageStore messageStore)
		{
			SubscriptionStore = subscriptionStore;
			MessageStore = messageStore;
		}

		public void Subscribe(ISubscription subscription)
		{
			if (!SubscriptionStore.TryAddSubscription(subscription))
			{
				Debug.LogWarning($"Subscription from {subscription.Subscriber} failed for message type: {subscription.MessageType} from Subscriber: {subscription.Subscriber.GetType()}");
			}
		}

		public void Unsubscribe(ISubscription subscription)
		{
			SubscriptionStore.DeleteSubscription(subscription);
		}

		public void PublishSingle<TMessage>(TMessage message) where TMessage : IMessage
		{
			if (message == null) // Invalid message
			{
				Debug.LogWarning($"Unable to publish null {typeof(TMessage)}");
				return;
			}

			List<ISubscription> subscriptions = new();

			if (SubscriptionStore.TryGetSubscriptions<IMessage>(out List<ISubscription> generalSubscriptions))
			{
				subscriptions.AddRange(generalSubscriptions);
			}
			if (SubscriptionStore.TryGetSubscriptions<TMessage>(out List<ISubscription> specificSubscriptions))
			{
				subscriptions.AddRange(specificSubscriptions);
			}

			foreach (ISubscription subscription in subscriptions)
			{
				if (subscription.MessageFilter?.Invoke(message) ?? true) // Null filter counts as no filter
				{
					subscription.Callback?.Invoke(message);
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
			MessageStore.AddMessage(key, message);
			
			// Same as publishing to everyone right now
			PublishSingle(message);
		}

		public bool TryGetPersistantMessage(IMessageKey key, out IMessage message)
		{
			return MessageStore.TryGetMessage(key, out message);
		}

		public void DeletePersistant<TKey>(TKey key) where TKey : IMessageKey
		{
			if (key == null)
				return;

			MessageStore.TryDeleteMessage(key);
		}
	}
}