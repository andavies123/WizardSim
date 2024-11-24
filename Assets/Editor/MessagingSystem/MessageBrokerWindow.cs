using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using MessagingSystem;
using UnityEditor;
using UnityEngine;

namespace Editor.MessagingSystem
{
	public class MessageBrokerWindow : EditorWindow
	{
		private const string LISTENING_MODE_LISTENING = "Listening";
		private const string LISTENING_MODE_STOPPED = "Stopped";

		private const string SORT_MODE_TYPE = "Message Type";
		private const string SORT_MODE_SENDER = "Sender";
		private const string SORT_MODE_LATEST = "Latest";
		private const string SORT_MODE_OLDEST = "Oldest";

		private readonly string[] _sortOptions =
		{
			SORT_MODE_TYPE,
			SORT_MODE_SENDER,
			SORT_MODE_LATEST,
			SORT_MODE_OLDEST
		};

		private readonly SortedMessageList[] _sortedMessageLists =
		{
			new MessageTypeSortedMessageList(),
			new SenderSortedMessageList(),
			new LatestMessageSortedMessageList(),
			new OldestMessageSortedMessageList()
		};
		
		private readonly List<MessageInfo> _allMessages = new();
		
		// Editor UI States
		private Dictionary<Type, bool> _persistantMessageFoldoutStates;
		private Vector2 _singleUseScrollPosition = Vector2.zero;
		private Vector2 _persistantScrollPosition = Vector2.zero;
		private string _searchStringPersistant = "";
		
		private MessageBroker _messageBroker;
		private IReadOnlyDictionary<Type, HashSet<MessagePair>> _cachedPersistantMessages;
		private string _currentListeningMode = LISTENING_MODE_STOPPED;

		private string _searchStringSingle = "";
		private string SearchStringSingle
		{
			get => _searchStringSingle;
			set
			{
				if (value != _searchStringSingle)
				{
					_searchStringSingle = value;
					Repaint();
				}
			}
		}

		private int _sortModeOptionSingle;
		private int SortModeOptionSingle
		{
			get => _sortModeOptionSingle;
			set
			{
				if (value != _sortModeOptionSingle)
				{
					_sortModeOptionSingle = value;
					Repaint();
				}
			}
		}
        
		[MenuItem("Window/Message Broker")]
		public static void ShowWindow()
		{
			GetWindow<MessageBrokerWindow>("Message Broker");
		}

		private void OnEnable()
		{
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		private void OnGUI()
		{
			foreach (SortedMessageList sortedMessageList in _sortedMessageLists)
			{
				sortedMessageList.CreateStyles();
			}

			EditorGUILayout.BeginVertical();
			
			DrawHeaderRow();
			
			EditorGUILayout.BeginHorizontal();
			
			//DrawPersistantColumn();
			//DrawVerticalSeparator(2);
			DrawSingleMessagesColumn();
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
		}

		private void DrawHeaderRow()
		{
			EditorGUILayout.BeginHorizontal();

			string messageBrokerString = _messageBroker == null ? "Does not exist" : "Exists";
			GUILayout.Label($"Listening Mode: {_currentListeningMode} \t Message Broker: {messageBrokerString}", EditorStyles.boldLabel);
			
			EditorGUILayout.EndHorizontal();
		}

		private void DrawPersistantColumn()
		{
			EditorGUILayout.BeginVertical();
			_persistantScrollPosition = EditorGUILayout.BeginScrollView(_persistantScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label("Persistant Messages", EditorStyles.boldLabel);
			_searchStringPersistant = EditorGUILayout.TextField("Search:", _searchStringPersistant, GUILayout.ExpandWidth(true));
			
			EditorGUILayout.EndHorizontal();

			if (_currentListeningMode == LISTENING_MODE_LISTENING && _messageBroker != null)
			{
				// Display live messages
				foreach (KeyValuePair<Type, HashSet<MessagePair>> messagePairsByType in _messageBroker.MessageStore.Messages)
				{
					_persistantMessageFoldoutStates.TryAdd(messagePairsByType.Key, false);
					_persistantMessageFoldoutStates[messagePairsByType.Key] = EditorGUILayout.Foldout(
						_persistantMessageFoldoutStates[messagePairsByType.Key], messagePairsByType.Key.Name);
					
					foreach (MessagePair messagePair in messagePairsByType.Value)
					{
						DrawMessageRow(messagePair);
					}
				}
			}
			else if (_currentListeningMode == LISTENING_MODE_STOPPED && _cachedPersistantMessages != null)
			{
                // Display cached messages
                foreach (KeyValuePair<Type, HashSet<MessagePair>> messagePairsByType in _cachedPersistantMessages)
                {
	                foreach (MessagePair messagePair in messagePairsByType.Value)
	                {
		                DrawMessageRow(messagePair);
	                }
                }
			}
			
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
		
		private void DrawSingleMessagesColumn()
		{
			EditorGUILayout.BeginVertical();
			_singleUseScrollPosition = EditorGUILayout.BeginScrollView(_singleUseScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

			EditorGUILayout.BeginHorizontal();
			
			GUILayout.Label("Single Messages", EditorStyles.boldLabel);
			SortModeOptionSingle = Popup("Sort By:", SortModeOptionSingle, _sortOptions);
			SearchStringSingle = TextField("Search:", SearchStringSingle);
			
			EditorGUILayout.EndHorizontal();

			// Display cached messages
			List<MessageInfo> searchFilteredMessages = _allMessages.Where(message => message.MatchesSearch(_searchStringSingle)).ToList();
			_sortedMessageLists[_sortModeOptionSingle].Draw(searchFilteredMessages);

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void DrawMessageRow(MessagePair messagePair)
		{
			GUILayout.Label($"Key: {messagePair.Key.GetType().Name} | Message: {messagePair.Message.GetType().Name}");
		}

		private void OnPlayModeStateChanged(PlayModeStateChange stateChange)
		{
			if (stateChange == PlayModeStateChange.EnteredPlayMode)
			{
				_currentListeningMode = LISTENING_MODE_LISTENING;
				_messageBroker = Dependencies.Get<MessageBroker>();
				_messageBroker.Subscribe(new Subscription(this, typeof(IMessage), null, OnMessageReceived));
				// Reset window state
				_allMessages.Clear();
			}
			else if (stateChange == PlayModeStateChange.ExitingPlayMode)
			{
				_currentListeningMode = LISTENING_MODE_STOPPED;
				// Cache messages from the system
				_cachedPersistantMessages = _messageBroker.MessageStore.Messages;
			}
		}

		private void OnMessageReceived(IMessage message)
		{
			_allMessages.Add(new MessageInfo
			{
				Message = message,
				TimeReceived = DateTime.Now
			});
			Repaint();
		}

		private static void DrawVerticalSeparator(int lineWidth)
		{
			GUILayout.Box(GUIContent.none, GUILayout.Width(lineWidth), GUILayout.ExpandHeight(true));
		}
		
		private static string TextField(string label, string text)
		{
			var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
			EditorGUIUtility.labelWidth = textDimensions.x;
			return EditorGUILayout.TextField(label, text, GUILayout.Width(300));
		}

		private static int Popup(string label, int selectedIndex, string[] displayOptions)
		{
			var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
			EditorGUIUtility.labelWidth = textDimensions.x;
			return EditorGUILayout.Popup(label, selectedIndex, displayOptions, GUILayout.Width(175));
		}
	}
}
