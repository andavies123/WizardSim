using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEditor;

namespace Editor.MessagingSystem
{
	public abstract class SortedPersistantMessageList : SortedBrokerList<PersistantMessageListItem> { }
	
	public class SenderSortedPersistantMessageList : SortedPersistantMessageList
	{
		public override string SortTypeName => "Sender";
		
		public override void Draw(List<PersistantMessageListItem> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;

			Dictionary<string, List<PersistantMessageListItem>> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.Message.Sender.GetType().Name)
				.GroupBy(messageInfo => messageInfo.Message.Sender.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string sender, List<PersistantMessageListItem> messageBySender) in sortedMessages)
			{
				if (!FoldoutStates.ContainsKey(sender))
				{
					FoldoutStates.TryAdd(sender, false);
				}
				
				FoldoutStates[sender] = EditorGUILayout.Foldout(FoldoutStates[sender], sender, FoldoutHeaderFontStyle);
				EditorExtensions.DrawHorizontalSeparator();
				
				if (FoldoutStates[sender])
				{
					foreach (PersistantMessageListItem messageInfo in messageBySender)
					{
						messageInfo.Draw();
					}
				}
			}
		}
	}

	public class MessageTypeSortedPersistantMessageList : SortedPersistantMessageList
	{
		public override string SortTypeName => "Message Type";
		
		public override void Draw(List<PersistantMessageListItem> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;

			Dictionary<string, List<PersistantMessageListItem>> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.Message.GetType().Name)
				.GroupBy(messageInfo => messageInfo.Message.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string messageType, List<PersistantMessageListItem> messageByType) in sortedMessages)
			{
				if (!FoldoutStates.ContainsKey(messageType))
				{
					FoldoutStates.TryAdd(messageType, false);
				}
				
				FoldoutStates[messageType] = EditorGUILayout.Foldout(FoldoutStates[messageType], messageType, FoldoutHeaderFontStyle);
				EditorExtensions.DrawHorizontalSeparator();
				
				if (FoldoutStates[messageType])
				{
					foreach (PersistantMessageListItem messageListItem in messageByType)
					{
						messageListItem.Draw();
					}
				}
			}
		}
	}

	public class KeyTypeSortedPersistantMessageList : SortedPersistantMessageList
	{
		public override string SortTypeName => "Key Type";
		
		public override void Draw(List<PersistantMessageListItem> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;

			Dictionary<string, List<PersistantMessageListItem>> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.Key.GetType().Name)
				.GroupBy(messageInfo => messageInfo.Key.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string messageType, List<PersistantMessageListItem> messageByKeyType) in sortedMessages)
			{
				if (!FoldoutStates.ContainsKey(messageType))
				{
					FoldoutStates.TryAdd(messageType, false);
				}
				
				FoldoutStates[messageType] = EditorGUILayout.Foldout(FoldoutStates[messageType], messageType, FoldoutHeaderFontStyle);
				EditorExtensions.DrawHorizontalSeparator();
				
				if (FoldoutStates[messageType])
				{
					foreach (PersistantMessageListItem messageListItem in messageByKeyType)
					{
						messageListItem.Draw();
					}
				}
			}
		}
	}
}