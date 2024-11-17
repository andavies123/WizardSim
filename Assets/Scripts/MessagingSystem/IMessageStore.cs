using System;
using System.Collections.Generic;

namespace MessagingSystem
{
	public interface IMessageStore
	{
		/// <summary>
		/// Contains all messages currently held in the system
		/// Key: Object type of the key
		/// Value: Collection of Key/Value pairs
		/// </summary>
		IReadOnlyDictionary<Type, HashSet<MessagePair>> Messages { get; }

		/// <summary>
		/// Adds a message to the message store.
		/// If the message exists, it will be updated
		/// </summary>
		/// <param name="key">The unique key for the message</param>
		/// <param name="message">The actual message object containing data</param>
		void AddMessage(IMessageKey key, IMessage message);

		/// <summary>
		/// Gets a message from the message store. Returns null if no such message exists
		/// </summary>
		/// <param name="key">The key used to find the message</param>
		/// <param name="message">The found message from the message store</param>
		/// <returns>True if a message was found. False if not</returns>
		bool TryGetMessage(IMessageKey key, out IMessage message);

		/// <summary>
		/// Deletes a single message from the message store
		/// </summary>
		/// <param name="key">The key that will be deleted from the message store</param>
		bool TryDeleteMessage(IMessageKey key);

		/// <summary>
		/// Deletes all messages in the message store that correspond with the given type
		/// </summary>
		/// <param name="messageType">The type of message to delete</param>
		/// <param name="deletedMessages">All the messages that were deleted</param>
		bool TryDeleteAllMessagesOfType(Type messageType, out List<MessagePair> deletedMessages);
	}
}