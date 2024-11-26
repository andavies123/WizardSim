using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEditor;

namespace Editor.MessagingSystem
{
	public abstract class SortedMessageList : SortedBrokerList<MessageListItem> { }

	public class SenderSortedMessageList : SortedMessageList
	{
		public override string SortTypeName => "Sender";
		
		public override void Draw(List<MessageListItem> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;

			Dictionary<string, List<MessageListItem>> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.Message.Sender.GetType().Name)
				.ThenByDescending(messageInfo => messageInfo.TimeReceived)
				.GroupBy(messageInfo => messageInfo.Message.Sender.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string sender, List<MessageListItem> messageBySender) in sortedMessages)
			{
				if (!FoldoutStates.ContainsKey(sender))
				{
					FoldoutStates.TryAdd(sender, false);
				}
				
				FoldoutStates[sender] = EditorGUILayout.Foldout(FoldoutStates[sender], sender, FoldoutHeaderFontStyle);
				EditorExtensions.DrawHorizontalSeparator();
				
				if (FoldoutStates[sender])
				{
					foreach (MessageListItem messageInfo in messageBySender)
					{
						messageInfo.Draw();
					}
				}
			}
		}
	}

	public class MessageTypeSortedMessageList : SortedMessageList
	{
		public override string SortTypeName => "Message Type";
		
		public override void Draw(List<MessageListItem> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;

			Dictionary<string, List<MessageListItem>> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.Message.GetType().Name)
				.ThenByDescending(messageInfo => messageInfo.TimeReceived)
				.GroupBy(messageInfo => messageInfo.Message.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string messageType, List<MessageListItem> messageByType) in sortedMessages)
			{
				if (!FoldoutStates.ContainsKey(messageType))
				{
					FoldoutStates.TryAdd(messageType, false);
				}
				
				FoldoutStates[messageType] = EditorGUILayout.Foldout(FoldoutStates[messageType], messageType, FoldoutHeaderFontStyle);
				EditorExtensions.DrawHorizontalSeparator();
				
				if (FoldoutStates[messageType])
				{
					foreach (MessageListItem messageInfo in messageByType)
					{
						messageInfo.Draw();
					}
				}
			}
		}
	}

	public class LatestMessageSortedMessageList : SortedMessageList
	{
		public override string SortTypeName => "Latest";
		
		public override void Draw(List<MessageListItem> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;
			
			List<MessageListItem> sortedMessages = messages
				.OrderByDescending(messageInfo => messageInfo.TimeReceived)
				.ToList();
			
			foreach (MessageListItem messageInfo in sortedMessages)
			{
				messageInfo.Draw();
			}
		}
	}

	public class OldestMessageSortedMessageList : SortedMessageList
	{
		public override string SortTypeName => "Oldest";
		
		public override void Draw(List<MessageListItem> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;
			
			List<MessageListItem> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.TimeReceived)
				.ToList();
			
			foreach (MessageListItem messageInfo in sortedMessages)
			{
				messageInfo.Draw();
			}
		}
	}
}