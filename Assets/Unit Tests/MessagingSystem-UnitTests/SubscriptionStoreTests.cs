using System.Collections.Generic;
using FluentAssertions;
using MessagingSystem;
using NSubstitute;
using NUnit.Framework;

namespace Unit_Tests.MessagingSystem_UnitTests
{
	[TestFixture]
	public class SubscriptionStoreTests
	{
		#region TryAddSubscription Tests

		// Adds Initial subscription
		
		[Test]
		public void TryAddSubscription_ReturnsTrue_WhenAddingANewSubscription()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription = GetMockSubscription<TestMessage>();
			
			// Act
			bool added = subscriptionStore.TryAddSubscription(subscription);

			// Assert
			added.Should().BeTrue();
			subscriptionStore.Subscriptions.Should().ContainKey(subscription.MessageType);
			subscriptionStore.Subscriptions[subscription.MessageType].Should().Contain(subscription);
		}
		
		// Does not add null subscription

		[Test]
		public void TryAddSubscription_ReturnsFalse_WhenAddingNullSubscription()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			
			// Act
			bool added = subscriptionStore.TryAddSubscription(null);

			// Assert
			added.Should().BeFalse();
			subscriptionStore.Subscriptions.Should().BeEmpty();
		}
		
		// Does not add if subscription already exists

		[Test]
		public void TryAddSubscription_ReturnsFalse_WhenAddingSubscriptionASecondTime()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription = GetMockSubscription<TestMessage>();
			
			// Act
			subscriptionStore.TryAddSubscription(subscription);
			bool added = subscriptionStore.TryAddSubscription(subscription);
			
			// Assert
			added.Should().BeFalse();
			subscriptionStore.Subscriptions[subscription.MessageType].Should().HaveCount(1);
		}
		
		// Able to add multiple same type subscriptions

		[Test]
		public void TryAddSubscriptions_AbleToAddMultipleSubscriptions_OfSameMessageType()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription1 = GetMockSubscription<TestMessage>();
			var subscription2 = GetMockSubscription<TestMessage>();
			
			// Act
			List<bool> subscriptionOutcomes = new()
			{
				subscriptionStore.TryAddSubscription(subscription1),
				subscriptionStore.TryAddSubscription(subscription2)
			};

			// Assert
			subscriptionOutcomes.Should().OnlyContain(outcome => outcome == true);
			subscriptionStore.Subscriptions.Should().ContainKey(subscription1.MessageType);
			subscriptionStore.Subscriptions[typeof(TestMessage)].Should().HaveCount(2);
			subscriptionStore.Subscriptions[typeof(TestMessage)].Should().Contain(subscription1);
			subscriptionStore.Subscriptions[typeof(TestMessage)].Should().Contain(subscription2);
		}
		
		// Able to add multiple different type subscriptions

		[Test]
		public void TryAddSubscriptions_AbleToAddMultipleSubscriptions_OfDifferentMessageType()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription1 = GetMockSubscription<TestMessage>();
			var subscription2 = GetMockSubscription<TestMessage2>();
			
			// Act
			List<bool> subscriptionOutcomes = new()
			{
				subscriptionStore.TryAddSubscription(subscription1),
				subscriptionStore.TryAddSubscription(subscription2)
			};

			// Assert
			subscriptionOutcomes.Should().OnlyContain(outcome => outcome == true);
			subscriptionStore.Subscriptions.Should().ContainKey(subscription1.MessageType);
			subscriptionStore.Subscriptions.Should().ContainKey(subscription2.MessageType);
			subscriptionStore.Subscriptions[typeof(TestMessage)].Should().HaveCount(1);
			subscriptionStore.Subscriptions[typeof(TestMessage)].Should().Contain(subscription1);
			subscriptionStore.Subscriptions[typeof(TestMessage2)].Should().HaveCount(1);
			subscriptionStore.Subscriptions[typeof(TestMessage2)].Should().Contain(subscription2);
		}

		#endregion

		#region DeleteSubscription Tests

		// Deletes the subscription that was added when using same object reference
		// Removes internal collection for message type when deleting all messages of type

		[Test]
		public void DeleteSubscription_RemovesSubscriptionFromCollection_WhenUsingSameReference()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription = GetMockSubscription<TestMessage>();
			
			// Act
			bool added = subscriptionStore.TryAddSubscription(subscription);
			subscriptionStore.DeleteSubscription(subscription);
            
			// Assert
			added.Should().BeTrue();
			subscriptionStore.Subscriptions.Should().NotContainKey(subscription.MessageType);
		}
		
		// Does not delete an exact match of subscription when using different object reference

		[Test]
		public void DeleteSubscription_DoesNotRemoveSubscriptionFromCollection_WhenUsingDifferentReference()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription1 = GetMockSubscription<TestMessage>();
			var subscription2 = GetMockSubscription<TestMessage>();
			
			// Act
			bool added = subscriptionStore.TryAddSubscription(subscription1);
			subscriptionStore.DeleteSubscription(subscription2);
			
			// Assert
			added.Should().BeTrue();
			subscriptionStore.Subscriptions.Should().ContainKey(subscription1.MessageType);
			subscriptionStore.Subscriptions[subscription1.MessageType].Should().Contain(subscription1);
		}
		
		// Does not remove internal collection for message type when subscription for type still exists

		[Test]
		public void DeleteSubscription_DoesNotDeleteAllSubscriptionsOfType_WhenMultipleExist()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription1 = GetMockSubscription<TestMessage>();
			var subscription2 = GetMockSubscription<TestMessage>();
			
			// Act
			List<bool> subscriptionOutcomes = new()
			{
				subscriptionStore.TryAddSubscription(subscription1),
				subscriptionStore.TryAddSubscription(subscription2)
			};
			
			subscriptionStore.DeleteSubscription(subscription1);

			// Assert
			subscriptionOutcomes.Should().OnlyContain(outcome => outcome == true);
			subscriptionStore.Subscriptions.Should().ContainKey(subscription2.MessageType);
			subscriptionStore.Subscriptions[subscription2.MessageType].Should().Contain(subscription2);
		}

		#endregion
		
		#region TryGetSubscriptions Tests
		
		// Gets single subscription if it exists

		[Test]
		public void TryGetSubscriptions_GrabsSubscription_WhenOneExists()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription = GetMockSubscription<TestMessage>();
			
			// Act
			subscriptionStore.TryAddSubscription(subscription);
			bool foundSubscription = subscriptionStore.TryGetSubscriptions<TestMessage>(out List<ISubscription> subscriptions);

			// Assert
			foundSubscription.Should().BeTrue();
			subscriptions.Should().HaveCount(1);
			subscriptions.Should().Contain(subscription);
		}
		
		// Gets all subscriptions of type when multiple exists

		[Test]
		public void TryGetSubscriptions_GrabsSubscription_WhenMultipleExist()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription1 = GetMockSubscription<TestMessage>();
			var subscription2 = GetMockSubscription<TestMessage>();
			
			// Act
			subscriptionStore.TryAddSubscription(subscription1);
			subscriptionStore.TryAddSubscription(subscription2);
			bool foundSubscriptions = subscriptionStore.TryGetSubscriptions<TestMessage>(out List<ISubscription> subscriptions);
			
			// Assert
			foundSubscriptions.Should().BeTrue();
			subscriptions.Should().HaveCount(2);
			subscriptions.Should().Contain(subscription1);
			subscriptions.Should().Contain(subscription2);
		}
		
		// Gets only subscriptions of type when multiple types exist

		[Test]
		public void TryGetSubscription_GrabsPartialSubscriptions_WhenMultipleTypesExist()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription1 = GetMockSubscription<TestMessage>();
			var subscription2 = GetMockSubscription<TestMessage2>();
			
			// Act
			subscriptionStore.TryAddSubscription(subscription1);
			subscriptionStore.TryAddSubscription(subscription2);
			bool foundSubscriptions = subscriptionStore.TryGetSubscriptions<TestMessage>(out List<ISubscription> subscriptions);
			
			// Assert
			foundSubscriptions.Should().BeTrue();
			subscriptions.Should().HaveCount(1);
			subscriptions.Should().Contain(subscription1);
			subscriptions.Should().NotContain(subscription2);
		}
		
		// Gets nothing when no subscriptions of type exist

		[Test]
		public void TryGetSubscription_GrabsNothing_WhenNoSubscriptionsOfTypeExist()
		{
			// Arrange
			var subscriptionStore = new SubscriptionStore();
			var subscription1 = GetMockSubscription<TestMessage>();
			
			// Act
			subscriptionStore.TryAddSubscription(subscription1);
			bool foundSubscriptions = subscriptionStore.TryGetSubscriptions<TestMessage2>(out List<ISubscription> subscriptions);

			// Assert
			foundSubscriptions.Should().BeFalse();
			subscriptions.Should().BeEmpty();
		}
		
		#endregion
		
		#region Test Objects

		private static ISubscription GetMockSubscription<TMessage>() where TMessage : IMessage
		{
			var subscription = Substitute.For<ISubscription>();
			subscription.MessageType.Returns(typeof(TMessage));
			return subscription;
		}
		
		private class TestMessage : IMessage { public object Sender => null; }
		private class TestMessage2 : IMessage { public object Sender => null; }
		
		#endregion
	}
}