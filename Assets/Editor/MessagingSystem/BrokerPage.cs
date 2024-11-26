using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.MessagingSystem
{
	public abstract class BrokerPage
	{
		protected int SortModeDropdownIndex;
		private Vector2 _scrollPosition;
		private string _searchString;

		protected List<BrokerListItem> FilteredListItems = new();

		public abstract SortedBrokerList CurrentSortedList { get; }
		public abstract string PageName { get; }
		protected abstract string[] SortOptions { get; }

		public void DrawPage(IEnumerable<BrokerListItem> listItems)
		{
			FilteredListItems = listItems.Where(listItem => listItem.MatchesSearch(_searchString)).ToList();
			
			EditorGUILayout.BeginVertical();
			
			_scrollPosition = EditorGUILayout.BeginScrollView(
				_scrollPosition,
				GUILayout.ExpandWidth(true), 
				GUILayout.ExpandHeight(true));
			
			DrawHeader();
			DrawContent();
			
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void DrawHeader()
		{
			EditorGUILayout.BeginHorizontal();
			
			GUILayout.Label(PageName, EditorStyles.boldLabel);
			
			if (GUILayout.Button("Expand All", GUILayout.MaxWidth(100)))
				CurrentSortedList.ExpandAll();
			if (GUILayout.Button("Collapse All", GUILayout.MaxWidth(100)))
				CurrentSortedList.CollapseAll();
			
			SortModeDropdownIndex = EditorExtensions.Dropdown("Sort By:", SortModeDropdownIndex, SortOptions, 175);
			_searchString = EditorExtensions.TextField("Search:", _searchString, 300);
			
			EditorGUILayout.EndHorizontal();
		}

		protected abstract void DrawContent();
	}

	public class SingleMessagePage : BrokerPage
	{
		private static readonly List<SortedMessageList> SortedMessageLists = new()
		{
			new MessageTypeSortedMessageList(),
			new SenderSortedMessageList(),
			new LatestMessageSortedMessageList(),
			new OldestMessageSortedMessageList()
		};

		public override SortedBrokerList CurrentSortedList => SortedMessageLists[SortModeDropdownIndex];
		public override string PageName => "Messages";
		
		protected override string[] SortOptions { get; } =
			SortedMessageLists.Select(list => list.SortTypeName).ToArray();

		protected override void DrawContent()
		{
			// Apparently these have to be called in "OnGUI" or else it throws an error
			SortedMessageLists.ForEach(list => list.CreateStyles());
			SortedMessageLists[SortModeDropdownIndex].Draw(FilteredListItems.Cast<MessageListItem>().ToList());
		}
	}

	public class SubscriptionPage : BrokerPage
	{
		private static readonly List<SortedSubscriptionList> SortedSubscriberLists = new()
		{
			new SubscriberSortedSubscriptionList(),
			new SubscriptionSortedSubscriptionList()
		};
		
		public override SortedBrokerList CurrentSortedList => SortedSubscriberLists[SortModeDropdownIndex];
		public override string PageName => "Subscribers";
		
		protected override string[] SortOptions { get; } =
			SortedSubscriberLists.Select(list => list.SortTypeName).ToArray();
		
		protected override void DrawContent()
		{
			SortedSubscriberLists.ForEach(list => list.CreateStyles());
			SortedSubscriberLists[SortModeDropdownIndex].Draw(FilteredListItems.Cast<SubscriptionListItem>().ToList());
		}
	}
}