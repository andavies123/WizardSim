using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEditor;
using UnityEngine;

namespace Editor.MessagingSystem
{
	public abstract class SortedMessageList
	{
		protected readonly Dictionary<string, bool> FoldoutStates = new();
		protected GUIStyle FoldoutHeaderFontStyle;
		
		public abstract void Draw(List<MessageInfo> messages);

		public void CreateStyles()
		{
			FoldoutHeaderFontStyle = new GUIStyle(EditorStyles.foldout)
			{
				fontSize = 15,
				fontStyle = FontStyle.Bold,
				normal = { textColor = Color.yellow },
				onNormal = {textColor = Color.green }
			};
		}

		protected static void DrawHorizontalSeparator(int lineWidth)
		{
			GUILayout.Box(GUIContent.none, GUILayout.Height(lineWidth), GUILayout.ExpandWidth(true));
		}
	}

	public class SenderSortedMessageList : SortedMessageList
	{
		public override void Draw(List<MessageInfo> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;

			Dictionary<string, List<MessageInfo>> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.Message.Sender.GetType().Name)
				.ThenByDescending(messageInfo => messageInfo.TimeReceived)
				.GroupBy(messageInfo => messageInfo.Message.Sender.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string sender, List<MessageInfo> messageBySender) in sortedMessages)
			{
				if (!FoldoutStates.ContainsKey(sender))
				{
					FoldoutStates.TryAdd(sender, false);
				}
				
				FoldoutStates[sender] = EditorGUILayout.Foldout(FoldoutStates[sender], sender, FoldoutHeaderFontStyle);
				DrawHorizontalSeparator(2);
				
				if (FoldoutStates[sender])
				{
					foreach (MessageInfo messageInfo in messageBySender)
					{
						messageInfo.Draw();
					}
				}
			}
		}
	}

	public class MessageTypeSortedMessageList : SortedMessageList
	{
		public override void Draw(List<MessageInfo> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;

			Dictionary<string, List<MessageInfo>> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.Message.GetType().Name)
				.ThenByDescending(messageInfo => messageInfo.TimeReceived)
				.GroupBy(messageInfo => messageInfo.Message.GetType().Name)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach ((string messageType, List<MessageInfo> messageByType) in sortedMessages)
			{
				if (!FoldoutStates.ContainsKey(messageType))
				{
					FoldoutStates.TryAdd(messageType, false);
				}
				
				FoldoutStates[messageType] = EditorGUILayout.Foldout(FoldoutStates[messageType], messageType, FoldoutHeaderFontStyle);
				DrawHorizontalSeparator(2);
				
				if (FoldoutStates[messageType])
				{
					foreach (MessageInfo messageInfo in messageByType)
					{
						messageInfo.Draw();
					}
				}
			}
		}
	}

	public class LatestMessageSortedMessageList : SortedMessageList
	{
		public override void Draw(List<MessageInfo> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;
			
			List<MessageInfo> sortedMessages = messages
				.OrderByDescending(messageInfo => messageInfo.TimeReceived)
				.ToList();
			
			foreach (MessageInfo messageInfo in sortedMessages)
			{
				messageInfo.Draw();
			}
		}
	}

	public class OldestMessageSortedMessageList : SortedMessageList
	{
		public override void Draw(List<MessageInfo> messages)
		{
			if (messages == null || messages.IsEmpty())
				return;
			
			List<MessageInfo> sortedMessages = messages
				.OrderBy(messageInfo => messageInfo.TimeReceived)
				.ToList();
			
			foreach (MessageInfo messageInfo in sortedMessages)
			{
				messageInfo.Draw();
			}
		}
	}
}