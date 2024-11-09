using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MessagingSystem
{
	/// <summary>
	/// Contains code related to storing global messages into a single "database" for easy querying
	/// </summary>
	internal sealed class MessageStore
	{
		/// <summary>
		/// Key: Object type of the key
		/// Value: Collection of Key/Value pairs
		/// </summary>
		private readonly ConcurrentDictionary<Type, HashSet<MessagePair>> _messages = new();
		
		/// <summary>
		/// Adds a message to the message store.
		/// If the message exists, it will be updated
		/// </summary>
		/// <param name="key">The unique key for the message</param>
		/// <param name="message">The actual message object containing data</param>
		public void AddMessage(IMessageKey key, IMessage message)
		{
			_messages.AddOrUpdate(
				key.GetType(),
				new HashSet<MessagePair> {new(key, message)}, 
				(_, existingMessages) =>
				{
					MessagePair existingPair = existingMessages.FirstOrDefault(x => x.Key.CompareString == key.CompareString);
					if (existingPair == null)
					{
						existingMessages.Add(new MessagePair(key, message));
					}
					else
					{
						existingPair.Message = message;
					}
					
					return existingMessages;
				});
		}

		/// <summary>
		/// Deletes a single message from the message store
		/// </summary>
		/// <param name="key">The key that will be deleted from the message store</param>
		public bool TryDeleteMessage(IMessageKey key)
		{
			if (!_messages.TryGetValue(key.GetType(), out HashSet<MessagePair> messagePairs))
			{
				return false; // Message of that type doesn't exist
			}

			MessagePair pairToRemove = messagePairs.FirstOrDefault(pair => pair.Key.CompareString == key.CompareString);

			if (pairToRemove == null)
			{
				return false; // Message not found
			}
			
			messagePairs.Remove(pairToRemove);
				
			if (messagePairs.Count == 0)
			{
				_messages.Remove(key.GetType(), out _);
			}

			return true;
		}

		/// <summary>
		/// Deletes all messages in the message store that correspond with the given type
		/// </summary>
		/// <param name="messageType">The type of message to delete</param>
		/// <param name="deletedMessages">All the messages that were deleted</param>
		public bool TryDeleteAllMessagesOfType(Type messageType, out List<MessagePair> deletedMessages)
		{
			deletedMessages = null;
			
			if (!_messages.TryRemove(messageType, out HashSet<MessagePair> messagePairs))
				return false;

			deletedMessages = messagePairs.ToList();
			return true;
		}
	}
	
	/// <summary>
	/// Data class to combine a key and a message
	/// </summary>
	internal sealed class MessagePair
	{
		public MessagePair(IMessageKey key, IMessage message)
		{
			Key = key;
			Message = message;
		}
		
		public IMessageKey Key { get; }
		public IMessage Message { get; set; }
	}
}