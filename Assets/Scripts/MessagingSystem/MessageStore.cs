using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MessagingSystem
{
	internal sealed class MessageStore : IMessageStore
	{
		private readonly ConcurrentDictionary<Type, HashSet<MessagePair>> _messages = new();

		public IReadOnlyDictionary<Type, HashSet<MessagePair>> Messages => _messages;
		
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

		public bool TryGetMessage(IMessageKey key, out IMessage message)
		{
			message = null;

			if (_messages.TryGetValue(key.GetType(), out HashSet<MessagePair> messagePairs))
			{
				message = messagePairs.FirstOrDefault(pair => pair.Key.CompareString == key.CompareString)?.Message;
				return message != null;
			}

			return false;
		}

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
	public sealed class MessagePair
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