using System;
using Extensions;
using MessagingSystem;
using UnityEditor;
using UnityEngine;

namespace Editor.MessagingSystem
{
	public abstract class BrokerListItem
	{
		public abstract void Draw();
		public abstract bool MatchesSearch(string searchString);
	}
	
	public class MessageListItem : BrokerListItem
	{
		public IMessage Message { get; set; }
		public DateTime TimeReceived { get; set; }

		public override bool MatchesSearch(string searchString) =>
			searchString.IsNullOrEmpty() ||
			Message.GetType().Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) ||
			Message.Sender.GetType().Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
		
		public override void Draw()
		{
			EditorGUILayout.BeginVertical();
			
			GUILayout.Label($"[{TimeReceived.ToLongTimeString()}]:\t{Message.GetType().Name}", EditorStyles.boldLabel);
			GUILayout.Label($"\t\tSender: {Message.Sender.GetType().Name}");
			EditorExtensions.DrawHorizontalSeparator();
			
			EditorGUILayout.EndVertical();
		}
	}

	public class SubscriptionListItem : BrokerListItem
	{
		public ISubscription Subscription { get; set; }
		
		public override bool MatchesSearch(string searchString) =>
			searchString.IsNullOrEmpty() ||
			Subscription.MessageType.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) ||
			Subscription.Subscriber.GetType().Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);

		public override void Draw()
		{
			EditorGUILayout.BeginVertical();
			
			GUILayout.Label($"\tSubscription: {Subscription.MessageType.Name}");
			GUILayout.Label($"\tSubscriber: {Subscription.Subscriber.GetType().Name}");
			GUILayout.Label($"\tHas Filter: {(Subscription.MessageFilter == null ? "No" : "Yes")}");
			EditorExtensions.DrawHorizontalSeparator();
			
			EditorGUILayout.EndVertical();
		}
	}
}