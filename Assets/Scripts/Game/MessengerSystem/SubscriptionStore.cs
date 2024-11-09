using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Game.MessengerSystem
{
	internal sealed class SubscriptionStore
	{
		private readonly ConcurrentDictionary<Type, HashSet<ISubscription>> _subscriptions = new();

		public bool TryAddSubscription(ISubscription subscription)
		{
			if (subscription == null)
				return false;
			
			if (_subscriptions.TryGetValue(subscription.MessageType, out HashSet<ISubscription> subscriptions))
			{
				lock (subscriptions)
				{
					return subscriptions.Add(subscription);
				}
			}
			
			_subscriptions[subscription.MessageType] = new HashSet<ISubscription> { subscription };
			return true;
		}

		public void DeleteSubscription(ISubscription subscription)
		{
			if (_subscriptions.TryGetValue(subscription.MessageType, out HashSet<ISubscription> subscriptions))
			{
				lock (subscriptions)
				{
					if (subscriptions.Remove(subscription) && subscriptions.Count == 0)
					{
						_subscriptions.TryRemove(subscription.MessageType, out _);
					}
				}
			}
		}

		public bool TryGetSubscriptions<TMessage>(out List<ISubscription> subscriptions) where TMessage : IMessage
		{
			if (_subscriptions.TryGetValue(typeof(TMessage), out HashSet<ISubscription> subscriptionsSet))
			{
				lock (subscriptionsSet)
				{
					subscriptions = subscriptionsSet.ToList();
					return true;
				}
			}

			subscriptions = null;
			return false;
		}
	}
}