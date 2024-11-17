using System;
using System.Collections.Generic;

namespace MessagingSystem
{
	public interface ISubscriptionStore
	{
		/// <summary>
		/// Collection of all subscriptions
		/// </summary>
		IReadOnlyDictionary<Type, HashSet<ISubscription>> Subscriptions { get; }
		
		/// <summary>
		/// Adds a subscription to the store. When a subscription exists in the store,
		/// it will receive updates from published messages
		/// </summary>
		/// <param name="subscription">The subscription to add</param>
		/// <returns>True if a subscription was added. False if no subscription was added (already exists)</returns>
		bool TryAddSubscription(ISubscription subscription);
		
		/// <summary>
		/// Deletes a subscription from the store. After a subscription is deleted,
		/// it won't receive any more updates from published messages
		/// </summary>
		/// <param name="subscription">Subscription to delete</param>
		void DeleteSubscription(ISubscription subscription);
		
		/// <summary>
		/// Tries to get all subscriptions of a message type from the internal dictionary if it exists
		/// </summary>
		/// <param name="subscriptions">Collection of subscriptions found</param>
		/// <typeparam name="TMessage">Type of message the subscriptions are based on</typeparam>
		/// <returns>True if subscriptions were found. False if no subscriptions were found</returns>
		bool TryGetSubscriptions<TMessage>(out List<ISubscription> subscriptions) where TMessage : IMessage;
	}
}