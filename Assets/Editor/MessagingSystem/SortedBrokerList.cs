using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.MessagingSystem
{
	public abstract class SortedBrokerList
	{
		protected readonly Dictionary<string, bool> FoldoutStates = new();
		protected GUIStyle FoldoutHeaderFontStyle;
		
		public abstract string SortTypeName { get; }
		
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

		public void ExpandAll() => FoldoutStates.Keys.ToList().ForEach(key => FoldoutStates[key] = true);
		public void CollapseAll() => FoldoutStates.Keys.ToList().ForEach(key => FoldoutStates[key] = false);	
	}
	
	public abstract class SortedBrokerList<TListItem> : SortedBrokerList where TListItem : BrokerListItem
	{
		public abstract void Draw(List<TListItem> listItems);
	}
}