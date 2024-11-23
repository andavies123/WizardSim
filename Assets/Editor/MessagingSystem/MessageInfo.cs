using System;
using Extensions;
using MessagingSystem;
using UnityEditor;
using UnityEngine;

namespace Editor.MessagingSystem
{
	public class MessageInfo
	{
		public IMessage Message { get; set; }
		public DateTime TimeReceived { get; set; }

		public bool MatchesSearch(string searchString) =>
			searchString.IsNullOrEmpty() ||
			Message.GetType().Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) ||
			Message.Sender.GetType().Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
		
		public void Draw()
		{
			EditorGUILayout.BeginVertical();
			GUILayout.Label($"[{TimeReceived.ToLongTimeString()}]:\t{Message.GetType().Name}", EditorStyles.boldLabel);
			GUILayout.Label($"\t\tSender: {Message.Sender.GetType().Name}");
			DrawHorizontalSeparator(2);
			EditorGUILayout.EndVertical();
		}

		private static void DrawHorizontalSeparator(int lineWidth)
		{
			GUILayout.Box(GUIContent.none, GUILayout.Height(lineWidth), GUILayout.ExpandWidth(true));
		}
	}
}