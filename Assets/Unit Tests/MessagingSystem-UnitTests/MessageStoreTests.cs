using System;
using System.Collections.Generic;
using FluentAssertions;
using MessagingSystem;
using NSubstitute;
using NUnit.Framework;

namespace Unit_Tests.MessagingSystem_UnitTests
{
	[TestFixture]
	public class MessageStoreTests
	{
		#region AddMessage Tests

		// Adds a key/message to the local message store
		
		[Test]
		public void AddMessage_AddsKeyAndMessageToStore_IfMessageDoesNotExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey();
			var message = Substitute.For<IMessage>();

			// Act
			messageStore.AddMessage(messageKey, message);

			// Assert
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey));
			messageStore.Messages[typeof(TestMessageKey)].Should().Contain(pair => pair.Key == messageKey && pair.Message == message);
		}
		
		// Adds a second key/message to the local message store when the compare strings don't match

		[Test]
		public void AddMessage_AddsTwoMessagesOfSameTypeToTheMessageStore_IfCompareStringsAreDifferent()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = "first message"};
			var messageKey2 = new TestMessageKey {CompareString = "second message"};
			var message = Substitute.For<IMessage>();

			// Act
			messageStore.AddMessage(messageKey, message);
			messageStore.AddMessage(messageKey2, message);

			// Assert
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey));
			messageStore.Messages[typeof(TestMessageKey)].Should().HaveCount(2);
			messageStore.Messages[typeof(TestMessageKey)].Should().Contain(pair => pair.Key == messageKey && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey)].Should().Contain(pair => pair.Key == messageKey2 && pair.Message == message);
		}
		
		// Does not add second message as a new message to the store when the compare strings match

		[Test]
		public void AddMessage_DoesNotAddSecondMessageOfSameType_IfCompareStringsIsEqual()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = "message"};
			var messageKey2 = new TestMessageKey {CompareString = "message"};
			var message = Substitute.For<IMessage>();
			
			// Act
			messageStore.AddMessage(messageKey, message);
			messageStore.AddMessage(messageKey2, message);
			
			// Assert
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey));
			messageStore.Messages[typeof(TestMessageKey)].Should().HaveCount(1);
			messageStore.Messages[typeof(TestMessageKey)].Should().Contain(pair => pair.Key == messageKey && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey)].Should().NotContain(pair => pair.Key == messageKey2 && pair.Message == message);
		}
		
		// Adds two messages of different types to the key store with different compare strings

		[Test]
		public void AddMessage_AddsTwoDifferentMessagesToStore_WhenCompareStringsAreDifferent()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = "message 1"};
			var messageKey2 = new TestMessageKey2 {CompareString = "message 2"};
			var message = Substitute.For<IMessage>();

			// Act
			messageStore.AddMessage(messageKey, message);
			messageStore.AddMessage(messageKey2, message);

			// Assert
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey));
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey2));
			messageStore.Messages[typeof(TestMessageKey)].Should().HaveCount(1);
			messageStore.Messages[typeof(TestMessageKey2)].Should().HaveCount(1);
			messageStore.Messages[typeof(TestMessageKey)].Should().Contain(pair => pair.Key == messageKey && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey2)].Should().Contain(pair => pair.Key == messageKey2 && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey)].Should().NotContain(pair => pair.Key == messageKey2 && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey2)].Should().NotContain(pair => pair.Key == messageKey && pair.Message == message);
		}
		
		// Adds two messages of different types to the key store with equal compare strings

		[Test]
		public void AddMessage_AddsTwoDifferentMessagesToStore_WhenCompareStringsAreSame()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = "message"};
			var messageKey2 = new TestMessageKey2 {CompareString = "message"};
			var message = Substitute.For<IMessage>();

			// Act
			messageStore.AddMessage(messageKey, message);
			messageStore.AddMessage(messageKey2, message);

			// Assert
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey));
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey2));
			messageStore.Messages[typeof(TestMessageKey)].Should().HaveCount(1);
			messageStore.Messages[typeof(TestMessageKey2)].Should().HaveCount(1);
			messageStore.Messages[typeof(TestMessageKey)].Should().Contain(pair => pair.Key == messageKey && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey2)].Should().Contain(pair => pair.Key == messageKey2 && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey)].Should().NotContain(pair => pair.Key == messageKey2 && pair.Message == message);
			messageStore.Messages[typeof(TestMessageKey2)].Should().NotContain(pair => pair.Key == messageKey && pair.Message == message);
		}
		
		// Updates the message in the store when message already exists and key's compare strings match

		[Test]
		public void AddMessage_UpdatesMessageInStore_WhenKeyAlreadyExists()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = "message"};
			var firstMessage = Substitute.For<IMessage>();
			var secondMessage = Substitute.For<IMessage>();
			
			// Act
			messageStore.AddMessage(messageKey, firstMessage);
			messageStore.AddMessage(messageKey, secondMessage);
			
			// Assert
			messageStore.Messages.Should().ContainKey(typeof(TestMessageKey));
			messageStore.Messages[typeof(TestMessageKey)].Should().HaveCount(1);
			messageStore.Messages[typeof(TestMessageKey)].Should().Contain(pair => pair.Key == messageKey && pair.Message == secondMessage);
			messageStore.Messages[typeof(TestMessageKey)].Should().NotContain(pair => pair.Key == messageKey && pair.Message == firstMessage);
		}
		
		#endregion
		
		#region TryGetMessage Tests
		
		// Correctly returns the message when the message exists in the store using same key object reference
		
		[Test]
		public void TryGetMessage_GetsMessageFromStore_WhenMessageExistsUsingSameKeyReference()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = "message"};
			var message = Substitute.For<IMessage>();
			
			// Act
			messageStore.AddMessage(messageKey, message);
			bool messageFound = messageStore.TryGetMessage(messageKey, out IMessage foundMessage);
			
			// Assert
			messageFound.Should().BeTrue();
			foundMessage.Should().BeSameAs(message);
		}
		
		// Correctly returns the message when the message exists in the store using a new key reference with similar values

		[Test]
		public void TryGetMessage_GetsMessageFromStore_WhenMessageExistsUsingNewKeyReference()
		{
			// Arrange
			var messageStore = new MessageStore();
			var originalMessageKey = new TestMessageKey {CompareString = "UnitTest"};
			var message = Substitute.For<IMessage>();

			// Act
			messageStore.AddMessage(originalMessageKey, message);
			bool found = messageStore.TryGetMessage(new TestMessageKey {CompareString = "UnitTest"}, out IMessage foundMessage);
			
			// Assert
			found.Should().BeTrue();
			foundMessage.Should().BeSameAs(message);
		}
		
		// Does not find the message when the message does not exist in the store

		[Test]
		public void TryGetMessage_DoesNotGetMessageFromStore_WhenMessageHasNotBeenAddedToStore()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = "Unit Test"};
			
			// Act
			bool found = messageStore.TryGetMessage(messageKey, out _);
			
			// Assert
			found.Should().BeFalse();
		}
		
		// Can correctly choose the right message when there are 2 messages of the same type with different keys

		[Test]
		public void TryGetMessage_CorrectlyGetsMessage_WhenMultipleOfSameTypeExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey1 = new TestMessageKey {CompareString = "Message1"};
			var messageKey2 = new TestMessageKey {CompareString = "Message2"};
			var message1 = Substitute.For<IMessage>();
			var message2 = Substitute.For<IMessage>();
			
			// Act
			messageStore.AddMessage(messageKey1, message1);
			messageStore.AddMessage(messageKey2, message2);
			bool found1 = messageStore.TryGetMessage(messageKey1, out IMessage foundMessage1);
			bool found2 = messageStore.TryGetMessage(messageKey2, out IMessage foundMessage2);
			
			// Assert
			found1.Should().BeTrue();
			found2.Should().BeTrue();
			foundMessage1.Should().BeSameAs(message1);
			foundMessage2.Should().BeSameAs(message2);
		}
		
		#endregion
		
		#region TryDeleteMessage Tests
		
		// Deletes a message from the store if it exists using same object reference

		[Test]
		public void TryDeleteMessage_DeletesMessageFromStore_UsingSameKeyReference()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};

			// Act
			messageStore.AddMessage(messageKey, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteMessage(messageKey);

			// Assert
			deleted.Should().BeTrue();
			messageStore.Messages.Should().NotContainKey(messageKey.GetType());
		}
		
		// Deletes a message from the store if it exists using a new object reference

		[Test]
		public void TryDeleteMessage_DeletesMessageFromStore_UsingDifferentKeyReference()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			messageStore.AddMessage(messageKey, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteMessage(new TestMessageKey {CompareString = messageKey.CompareString});
			
			// Assert
			deleted.Should().BeTrue();
			messageStore.Messages.Should().NotContainKey(messageKey.GetType());
		}
		
		// Does not delete a message from the store if it does not exist

		[Test]
		public void TryDeleteMessage_DoesNotDeleteMessage_IfTheMessageDoesNotExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			bool deleted = messageStore.TryDeleteMessage(messageKey);

			// Assert
			deleted.Should().BeFalse();
			messageStore.Messages.Should().NotContainKey(messageKey.GetType());
		}
		
		// Correctly deletes a message from the store when two of the same type exist

		[Test]
		public void TryDeleteMessage_DeletesSingleMessage_WhenMultipleMessageOfTypeExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey1 = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			var messageKey2 = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			messageStore.AddMessage(messageKey1, Substitute.For<IMessage>());
			messageStore.AddMessage(messageKey2, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteMessage(messageKey1);
			
			// Assert
			deleted.Should().BeTrue();
			messageStore.Messages.Should().ContainKey(messageKey2.GetType());
			messageStore.Messages[messageKey2.GetType()].Should().HaveCount(1);
			messageStore.Messages[messageKey2.GetType()].Should().Contain(pair => pair.Key == messageKey2);
			messageStore.Messages[messageKey1.GetType()].Should().NotContain(pair => pair.Key == messageKey1);
		}
		
		// Correctly deletes a message from the store when two different types exist

		[Test]
		public void TryDeleteMessage_DeletesSingleMessage_WhenMultipleTypesOfMessagesExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey1 = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			var messageKey2 = new TestMessageKey2 {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			messageStore.AddMessage(messageKey1, Substitute.For<IMessage>());
			messageStore.AddMessage(messageKey2, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteMessage(messageKey1);
			
			// Assert
			deleted.Should().BeTrue();
			messageStore.Messages.Should().NotContainKey(messageKey1.GetType());
			messageStore.Messages.Should().ContainKey(messageKey2.GetType());
			messageStore.Messages[messageKey2.GetType()].Should().HaveCount(1);
			messageStore.Messages[messageKey2.GetType()].Should().Contain(pair => pair.Key == messageKey2);
		}
		
		#endregion
		
		#region TryDeleteAllMessagesOfType Tests
		
		// Deletes a single message from store when only one exists

		[Test]
		public void TryDeleteAllMessagesOfType_DeletesSingleMessage_WhenOnlyOneExistsOfType()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			messageStore.AddMessage(messageKey, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteAllMessagesOfType(messageKey.GetType(), out List<MessagePair> deletedMessages);
			
			// Assert
			deleted.Should().BeTrue();
			deletedMessages.Should().HaveCount(1);
			deletedMessages.Should().Contain(pair => pair.Key == messageKey);
		}
		
		// Deletes all of same type of message from store when more than one exists

		[Test]
		public void TryDeleteAllMessagesOfType_DeletesAllMessagesOfType_WhenMultipleExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey1 = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			var messageKey2 = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			var messageKey3 = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			messageStore.AddMessage(messageKey1, Substitute.For<IMessage>());
			messageStore.AddMessage(messageKey2, Substitute.For<IMessage>());
			messageStore.AddMessage(messageKey3, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteAllMessagesOfType(typeof(TestMessageKey), out List<MessagePair> deletedMessages);
			
			// Assert
			deleted.Should().BeTrue();
			deletedMessages.Should().HaveCount(3);
			deletedMessages.Should().Contain(pair => pair.Key == messageKey1);
			deletedMessages.Should().Contain(pair => pair.Key == messageKey2);
			deletedMessages.Should().Contain(pair => pair.Key == messageKey3);
		}
		
		// Deletes only messages of given type from store when multiple types exist

		[Test]
		public void TryDeleteAllMessagesOfType_DeletesMessagesOfType_WhenMultipleTypesExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey1 = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			var messageKey2 = new TestMessageKey2 {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			messageStore.AddMessage(messageKey1, Substitute.For<IMessage>());
			messageStore.AddMessage(messageKey2, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteAllMessagesOfType(typeof(TestMessageKey), out List<MessagePair> deletedMessages);
			
			// Assert
			deleted.Should().BeTrue();
			deletedMessages.Should().HaveCount(1);
			deletedMessages.Should().Contain(pair => pair.Key == messageKey1);
			deletedMessages.Should().NotContain(pair => pair.Key == messageKey2);
			messageStore.Messages.Should().NotContainKey(messageKey1.GetType());
			messageStore.Messages.Should().ContainKey(messageKey2.GetType());
		}
		
		// Does not delete any messages of type when no message of type exist

		[Test]
		public void TryDeleteAllMessagesOfType_DoesNotDeleteAny_WhenTypeDoesNotExist()
		{
			// Arrange
			var messageStore = new MessageStore();
			var messageKey = new TestMessageKey {CompareString = Guid.NewGuid().ToString()};
			
			// Act
			messageStore.AddMessage(messageKey, Substitute.For<IMessage>());
			bool deleted = messageStore.TryDeleteAllMessagesOfType(typeof(TestMessageKey2), out List<MessagePair> deletedMessages);
			
			// Assert
			deleted.Should().BeFalse();
			deletedMessages.Should().BeNull();
		}
		
		#endregion
		
		#region Test Objects

		private class TestMessageKey : IMessageKey
		{
			public string CompareString { get; set; } = "";
			public object Sender => null;
		}

		private class TestMessageKey2 : IMessageKey
		{
			public string CompareString { get; set; } = "";
			public object Sender => null;
		}

		#endregion
	}
}