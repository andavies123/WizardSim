﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Game.MessengerSystem
{
	public static class GlobalMessenger
	{
		private static readonly ConcurrentDictionary<Type, HashSet<CallbackWrapper>> Subscriptions = new();
		private static readonly object SubscriptionLock = new();

		public static void Subscribe<T>(Action<T> callback) where T : IMessage
		{
			lock (SubscriptionLock)
			{
				CallbackWrapper callbackWrapper = new(callback);
				
				Subscriptions.AddOrUpdate(
					typeof(T),
					_ => new HashSet<CallbackWrapper> {callbackWrapper},
					(_, hashSet) =>
					{
						hashSet.Add(callbackWrapper);
						return hashSet;
					});
			}
		}

		public static void Unsubscribe<T>(Action<T> callback) where T : IMessage
		{
			lock (SubscriptionLock)
			{
				if (!Subscriptions.TryGetValue(typeof(T), out HashSet<CallbackWrapper> callbacks))
					return; // Subscription doesn't exist

				// Remove the callback from the internal collection
				callbacks.RemoveWhere(x => x.Callback.Target == callback.Target);
			
				// Handle some cleanup
				if (callbacks.Count == 0)
					Subscriptions.Remove(typeof(T), out _);
			}
		}

		public static void Publish<T>(T message) where T : IMessage
		{
			List<CallbackWrapper> callbacksCopy;
			
			lock (SubscriptionLock)
			{
				if (!Subscriptions.TryGetValue(typeof(T), out HashSet<CallbackWrapper> callbacks))
					return; // There are no subscriptions so there is no point of publishing

				// Copy is created so there aren't any issues when looping through callbacks and the collection gets updated
				callbacksCopy = callbacks.ToList();
			}
			
			// Publish message to each callback
			foreach (CallbackWrapper callback in callbacksCopy)
			{
				callback.Callback?.DynamicInvoke(message);
			}
		}

		private class CallbackWrapper
		{
			public CallbackWrapper(Delegate callback)
			{
				Callback = callback;
			}
			
			public Delegate Callback { get; }
		}
	}
}