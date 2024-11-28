using System;
using System.Text;
using FluentAssertions;
using MessagingSystem;
using NSubstitute;
using NUnit.Framework;

namespace Unit_Tests.MessagingSystem_UnitTests
{
	[TestFixture]
	[Parallelizable(ParallelScope.None)]
	public class MessageBrokerTests
	{
		#region Subscribe Tests
		
		// Correctly stores subscription

		[Test]
		public void Subscribe_CorrectlyStoresSubscription_WhenSubscriptionDoesNotAlreadyExist()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			var subscription = CreateMockSubscription(typeof(TestMessage));

			// Act
			messageBroker.Subscribe(subscription);

			// Assert
			subscriptionStore.Received(1).TryAddSubscription(Arg.Is(subscription));
		}
		
		// Correctly stores second subscription of same message type

		[Test]
		public void Subscribe_CorrectlyStoresMultipleSubscriptions()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			var subscription1 = CreateMockSubscription(typeof(TestMessage));
			var subscription2 = CreateMockSubscription(typeof(TestMessage));
			
			// Act
			messageBroker.Subscribe(subscription1);
			messageBroker.Subscribe(subscription2);
			
			// Assert
			subscriptionStore.Received(1).TryAddSubscription(Arg.Is(subscription1));
			subscriptionStore.Received(1).TryAddSubscription(Arg.Is(subscription2));
		}
		
		// Correctly stores second subscription of different message type

		[Test]
		public void Subscribe_CorrectlyStoresSubscription_WhenADifferentSubscriptionExistsOfDifferentMessageType()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			var subscription1 = CreateMockSubscription(typeof(TestMessage));
			var subscription2 = CreateMockSubscription(typeof(TestMessage2));
			
			// Act
			messageBroker.Subscribe(subscription1);
			messageBroker.Subscribe(subscription2);
			
			// Assert
			subscriptionStore.Received(1).TryAddSubscription(Arg.Is(subscription1));
			subscriptionStore.Received(1).TryAddSubscription(Arg.Is(subscription2));
		}
		
		#endregion
		
		#region Unsubscribe Tests
		
		// Correctly unsubscribes when the subscription exists

		[Test]
		public void Unsubscribe_CorrectlyUnsubscribes_WhenTheSubscriptionExists()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			var subscription = CreateMockSubscription(typeof(TestMessage));

			// Act
			messageBroker.Subscribe(subscription);
			messageBroker.Unsubscribe(subscription);

			// Assert
			subscriptionStore.Received().DeleteSubscription(Arg.Is(subscription));
		}
		
		// Correctly unsubscribes from multiple messages of same type

		[Test]
		public void Unsubscribe_CorrectlyUnsubscribes_WhenUnsubscribingFromMultipleSubscriptions_OfSameMessageType()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			var subscription1 = CreateMockSubscription(typeof(TestMessage));
			var subscription2 = CreateMockSubscription(typeof(TestMessage));
			var subscription3 = CreateMockSubscription(typeof(TestMessage));
			
			// Act
			messageBroker.Subscribe(subscription1);
			messageBroker.Subscribe(subscription2);
			messageBroker.Subscribe(subscription3);
			messageBroker.Unsubscribe(subscription1);
			messageBroker.Unsubscribe(subscription3);
			
			// Assert
			subscriptionStore.Received().DeleteSubscription(Arg.Is(subscription1));
			subscriptionStore.DidNotReceive().DeleteSubscription(Arg.Is(subscription2));
			subscriptionStore.Received().DeleteSubscription(Arg.Is(subscription3));
		}
		
		// Correctly unsubscribes from multiple messages of different types

		[Test]
		public void Unsubscribe_CorrectlyUnsubscribes_WhenUnsubscribingFromMultipleSubscriptions_OfDifferentMessageTypes()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			var subscription1 = CreateMockSubscription(typeof(TestMessage));
			var subscription2 = CreateMockSubscription(typeof(TestMessage));
			var subscription3 = CreateMockSubscription(typeof(TestMessage2));
			var subscription4 = CreateMockSubscription(typeof(TestMessage2));
			
			// Act
			messageBroker.Subscribe(subscription1);
			messageBroker.Subscribe(subscription2);
			messageBroker.Subscribe(subscription3);
			messageBroker.Subscribe(subscription4);
			messageBroker.Unsubscribe(subscription1);
			messageBroker.Unsubscribe(subscription3);
			
			// Assert
			subscriptionStore.Received().DeleteSubscription(Arg.Is(subscription1));
			subscriptionStore.DidNotReceive().DeleteSubscription(Arg.Is(subscription2));
			subscriptionStore.Received().DeleteSubscription(Arg.Is(subscription3));
			subscriptionStore.DidNotReceive().DeleteSubscription(Arg.Is(subscription4));
		}
		
		#endregion
		
		#region PublishSingle Tests
		
		// Message does not get added to the message store

		[Test]
		public void PublishSingle_MessageDoesNotGetAddedToMessageStore()
		{
			// Arrange
			var messageStore = MockMessageStore;
			var messageBroker = new MessageBroker(MockSubscriptionStore, messageStore);

			// Act
			messageBroker.PublishSingle(new TestMessage());

			// Assert
			messageStore.DidNotReceive().AddMessage(Arg.Any<IMessageKey>(), Arg.Any<IMessage>());
		}
		
		// Message Broker grabs subscribers to that message type

		[Test]
		public void PublishSingle_BrokerGrabsAllSubscribers_RelatedToMessageType()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			
			// Act
			messageBroker.PublishSingle(new TestMessage());
			
			// Assert
			subscriptionStore.Received().TryGetSubscriptions<TestMessage>(out _);
		}
		
		// Subscribers of different message type does not receive message

		[Test]
		public void PublishSingle_BrokerDoesNotGrabSubscriptions_OfDifferentType()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			
			// Act
			messageBroker.PublishSingle(new TestMessage());
			
			// Assert
			subscriptionStore.DidNotReceive().TryGetSubscriptions<TestMessage2>(out _);
		}
		
		// Message doesn't get published if null

		[Test]
		public void PublishSingle_BrokerDoesNotPublish_WhenMessageIsNull()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			
			// Act
			messageBroker.PublishSingle((TestMessage)null);
			
			// Assert
			subscriptionStore.DidNotReceive().TryGetSubscriptions<TestMessage>(out _);
		}
		
		#endregion
		
		#region PublishPersistant Tests
		
		// Broker adds message to the message store

		[Test]
		public void PublishPersistant_AddsMessageToMessageStore()
		{
			// Arrange
			var messageStore = MockMessageStore;
			var messageBroker = new MessageBroker(MockSubscriptionStore, messageStore);
			
			// Act
			messageBroker.PublishPersistant(MockMessageKey, MockMessage);
			
			// Assert
			messageStore.Received().AddMessage(Arg.Any<IMessageKey>(), Arg.Any<IMessage>());
		}
		
		// Broker does not add message to the message store if key is null

		[Test]
		public void PublishPersistant_DoesNotAddMessageToMessageStore_IfMessageKeyIsNull()
		{
			// Arrange
			var messageStore = MockMessageStore;
			var messageBroker = new MessageBroker(MockSubscriptionStore, messageStore);
			
			// Act
			messageBroker.PublishPersistant((IMessageKey)null, MockMessage);
			
			// Assert
			messageStore.DidNotReceive().AddMessage(Arg.Any<IMessageKey>(), Arg.Any<IMessage>());
		}
		
		// Broker does not add message to the message store if message is null

		[Test]
		public void PublishPersistant_DoesNotAddMessageToMessageStore_IfMessageIsNull()
		{
			// Arrange
			var messageStore = MockMessageStore;
			var messageBroker = new MessageBroker(MockSubscriptionStore, messageStore);
			
			// Act
			messageBroker.PublishPersistant(MockMessageKey, (IMessage)null);
			
			// Assert
			messageStore.DidNotReceive().AddMessage(Arg.Any<IMessageKey>(), Arg.Any<IMessage>());
		}
		
		// Broker grabs subscriptions of message type from the subscription store

		[Test]
		public void PublishPersistant_GrabsSubscriptionsOfMessageType()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			
			// Act
			messageBroker.PublishPersistant(MockMessageKey, MockMessage);
			
			// Assert
			subscriptionStore.Received().TryGetSubscriptions<IMessage>(out _);
		}
		
		// Broker does not grab subscriptions if key is null

		[Test]
		public void PublishPersistant_DoesNotGrabSubscriptions_WhenKeyIsNull()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			
			// Act
			messageBroker.PublishPersistant((IMessageKey)null, MockMessage);
			
			// Assert
			subscriptionStore.DidNotReceive().TryGetSubscriptions<IMessage>(out _);
		}
		
		// Broker does not grab subscriptions if message is null

		[Test]
		public void PublishPersistant_DoesNotGrabSubscriptions_WhenMessageIsNull()
		{
			// Arrange
			var subscriptionStore = MockSubscriptionStore;
			var messageBroker = new MessageBroker(subscriptionStore, MockMessageStore);
			
			// Act
			messageBroker.PublishPersistant(MockMessageKey, (IMessage)null);
			
			// Assert
			subscriptionStore.DidNotReceive().TryGetSubscriptions<IMessage>(out _);
		}
		
		#endregion
		
		#region TryGetPersistantMessage Tests
		
		// Gets a message if it exists in the message store

		[Test]
		public void TryGetPersistantMessage_CorrectlyGetsExistingMessage_FromMessageStore()
		{
			// Arrange
			var message = MockMessage;
			var messageKey = MockMessageKey;
			var messageStore = MockMessageStore;
			messageStore.TryGetMessage(messageKey, out Arg.Any<IMessage>()).Returns(x =>
			{
				x[1] = message;
				return true;
			});
			var messageBroker = new MessageBroker(MockSubscriptionStore, messageStore);

			// Act
			bool found = messageBroker.TryGetPersistantMessage(messageKey, out IMessage foundMessage);

			// Assert
			found.Should().BeTrue();
			foundMessage.Should().BeSameAs(message);
			messageStore.Received().TryGetMessage(Arg.Is(messageKey), out _);
		}
		
		#endregion
		
		#region DeletePersistantMessage Tests
		
		// Correctly deletes existing message from message store

		[Test]
		public void DeletePersistantMessage_DeletesMessageFromStore()
		{
			// Arrange
			var messageStore = MockMessageStore;
			var messageBroker = new MessageBroker(MockSubscriptionStore, messageStore);
			
			// Act
			messageBroker.DeletePersistant(MockMessageKey);
			
			// Assert
			messageStore.Received().TryDeleteMessage(Arg.Any<IMessageKey>());
		}

		// Does not delete message from message store when given a null key

		[Test]
		public void DeletePersistantMessage_DoesNotDeleteMessageFromStore_WhenKeyIsNull()
		{
			// Arrange
			var messageStore = MockMessageStore;
			var messageBroker = new MessageBroker(MockSubscriptionStore, messageStore);
			
			// Act
			messageBroker.DeletePersistant((IMessageKey)null);
			
			// Assert
			messageStore.DidNotReceive().TryDeleteMessage(Arg.Any<IMessageKey>());
		}
		
		#endregion
		
		#region Test Objects

		private static ISubscriptionStore MockSubscriptionStore => Substitute.For<ISubscriptionStore>();
		private static IMessageStore MockMessageStore => Substitute.For<IMessageStore>();
		
		private static ISubscription CreateMockSubscription(Type messageType)
		{
			var subscription = Substitute.For<ISubscription>();
			subscription.MessageType.Returns(messageType);
			return subscription;
		}

		private static IMessageKey MockMessageKey => Substitute.For<IMessageKey>();
		private static IMessage MockMessage => Substitute.For<IMessage>();

		private class TestMessage : IMessage
		{
			public object Sender => null;
			public StringBuilder GetDisplayText() => new();
		}

		private class TestMessage2 : IMessage
		{
			public object Sender => null;
			public StringBuilder GetDisplayText() => new();
		}

		#endregion
	}
}