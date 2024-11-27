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
		
		private readonly List<MessageListItem> _allMessages = new();
		private readonly List<PersistantMessageListItem> _allPersistantMessages = new();
		private readonly List<SubscriptionListItem> _allSubscriptions = new();
		private readonly List<BrokerPage> _pages = new()
		{
			new AllMessagesPage(),
			new PersistantMessagesPage(),
			new SubscriptionPage()
		};
		
		// Editor UI States
		private Dictionary<Type, bool> _persistantMessageFoldoutStates;
		
		private MessageBroker _messageBroker;
		private string _currentListeningMode = LISTENING_MODE_STOPPED;
		private int _currentPageIndex ;

		private BrokerPage CurrentPage => _pages[_currentPageIndex];
        
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
			if (_messageBroker is not null)
			{
				_allSubscriptions.Clear();
				foreach (KeyValuePair<Type, HashSet<ISubscription>> kvp in _messageBroker.SubscriptionStore.Subscriptions)
				{
					foreach (ISubscription subscription in kvp.Value)
					{
						_allSubscriptions.Add(new SubscriptionListItem
						{
							Subscription = subscription
						});
					}
				}

				_allPersistantMessages.Clear();
				foreach (KeyValuePair<Type, HashSet<MessagePair>> kvp in _messageBroker.MessageStore.Messages)
				{
					foreach (MessagePair messagePair in kvp.Value)
					{
						_allPersistantMessages.Add(new PersistantMessageListItem
						{
							Key = messagePair.Key,
							Message = messagePair.Message
						});
					}
				}
			}
			
			EditorGUILayout.BeginVertical();
			
			DrawHeaderRow();

			switch (CurrentPage)
			{
				case AllMessagesPage singleMessagePage:
					singleMessagePage.DrawPage(_allMessages);
					break;
				case PersistantMessagesPage persistantMessagesPage:
					persistantMessagesPage.DrawPage(_allPersistantMessages);
					break;
				case SubscriptionPage subscriptionPage:
					subscriptionPage.DrawPage(_allSubscriptions);
					break;
			}
			
			EditorGUILayout.EndVertical();
		}

		private void DrawHeaderRow()
		{
			EditorGUILayout.BeginHorizontal();

			string messageBrokerString = _messageBroker == null ? "Does not exist" : "Exists";
			GUILayout.Label($"Listening Mode: {_currentListeningMode} \t Message Broker: {messageBrokerString}", EditorStyles.boldLabel);
			_currentPageIndex = EditorExtensions.Dropdown(
				"Page:",
				_currentPageIndex,
				_pages.Select(page => page.PageName).ToArray(),
				175);
			
			EditorGUILayout.EndHorizontal();
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
				//_cachedPersistantMessages = _messageBroker.MessageStore.Messages;
			}
		}

		private void OnMessageReceived(IMessage message)
		{
			_allMessages.Add(new MessageListItem
			{
				Message = message,
				TimeReceived = DateTime.Now
			});
            
			Repaint();
		}
	}
}
