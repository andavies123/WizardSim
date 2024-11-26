using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEditor;

namespace Editor.MessagingSystem
{
	public abstract class SortedSubscriptionList : SortedBrokerList<SubscriptionListItem> { }

	public class SubscriberSortedSubscriptionList : SortedSubscriptionList
	{
		public override string SortTypeName => "Subscriber";
		
		public override void Draw(List<SubscriptionListItem> listItems)
		{
			if (listItems == null || listItems.IsEmpty())
				return;

			Dictionary<string, List<SubscriptionListItem>> sortedSubscriptions = listItems
				.OrderBy(listItem => listItem.Subscription.Subscriber.GetType().Name)
				.ThenByDescending(listItem => listItem.Subscription.MessageType.Name)
				.GroupBy(listItem => listItem.Subscription.Subscriber.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string subscriber, List<SubscriptionListItem> subscriptionBySubscriber) in sortedSubscriptions)
			{
				if (!FoldoutStates.ContainsKey(subscriber))
				{
					FoldoutStates.TryAdd(subscriber, false);
				}
				
				FoldoutStates[subscriber] = EditorGUILayout.Foldout(FoldoutStates[subscriber], subscriber, FoldoutHeaderFontStyle);
				EditorExtensions.DrawHorizontalSeparator();
				
				if (FoldoutStates[subscriber])
				{
					foreach (SubscriptionListItem messageInfo in subscriptionBySubscriber)
					{
						messageInfo.Draw();
					}
				}
			}
		}
	}
	
	public class SubscriptionSortedSubscriptionList : SortedSubscriptionList
	{
		public override string SortTypeName => "Subscription";
		
		public override void Draw(List<SubscriptionListItem> listItems)
		{
			if (listItems == null || listItems.IsEmpty())
				return;

			Dictionary<string, List<SubscriptionListItem>> sortedSubscriptions = listItems
				.OrderBy(listItem => listItem.Subscription.MessageType.Name)
				//.ThenByDescending(listItem => listItem.Subscription.MessageType.Name)
				.GroupBy(listItem => listItem.Subscription.MessageType.Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string subscription, List<SubscriptionListItem> subscriptionBySubscription) in sortedSubscriptions)
			{
				if (!FoldoutStates.ContainsKey(subscription))
				{
					FoldoutStates.TryAdd(subscription, false);
				}
				
				FoldoutStates[subscription] = EditorGUILayout.Foldout(FoldoutStates[subscription], subscription, FoldoutHeaderFontStyle);
				EditorExtensions.DrawHorizontalSeparator();
				
				if (FoldoutStates[subscription])
				{
					foreach (SubscriptionListItem messageInfo in subscriptionBySubscription)
					{
						messageInfo.Draw();
					}
				}
			}
		}
	}
}