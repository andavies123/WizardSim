using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Game.MessengerSystem
{
	public static class GlobalMessenger
	{
		private static readonly ConcurrentDictionary<Type, HashSet<CallbackWrapper>> Subscriptions = new();

		public static void Subscribe<T>(Action<T> callback) where T : IMessage
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

		public static void Unsubscribe<T>(Action<T> callback) where T : IMessage
		{
			if (!Subscriptions.TryGetValue(typeof(T), out HashSet<CallbackWrapper> callbacks))
				return; // Subscription doesn't exist

			// Remove the callback from the internal collection
			callbacks.Remove(new CallbackWrapper(callback));
			
			// Handle some cleanup
			if (callbacks.Count == 0)
				Subscriptions.Remove(typeof(T), out _);
		}

		public static void Publish<T>(T message) where T : IMessage
		{
			if (!Subscriptions.TryGetValue(typeof(T), out HashSet<CallbackWrapper> callbacks))
				return; // There are no subscriptions so there is no point of publishing

			// Publish message to each callback
			foreach (CallbackWrapper callback in callbacks)
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