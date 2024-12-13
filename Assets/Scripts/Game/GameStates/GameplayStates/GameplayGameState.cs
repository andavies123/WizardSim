using System;
using System.Collections.Generic;
using CameraComponents;
using Extensions;
using GameWorld.WorldObjects;
using Messages.Selection;
using Messages.UI;
using Messages.UI.Enums;
using MessagingSystem;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace Game.GameStates.GameplayStates
{
	public class GameplayGameState : GameState
	{
		private readonly GameplayUIState _gameplayUIState;
		private readonly GameplayInputState _gameplayInputState;
		private readonly InteractableRaycaster _interactableRaycaster;
		private readonly List<ISubscription> _subscriptions = new();
		private readonly MessageBroker _messageBroker;

		public event EventHandler PauseGameRequested;
		public event EventHandler<(ContextMenuUser, Vector2)> OpenContextMenuRequested;
		public event EventHandler<BeginPlacementModeEventArgs> BeginPlacementModeRequested;
		public event EventHandler OpenTownManagementWindow;
		public event EventHandler OpenTaskManagementWindow
		{
			add => _gameplayInputState.OpenTaskManagementRequested += value;
			remove => _gameplayInputState.OpenTaskManagementRequested -= value;
		}

		public GameplayGameState(GameplayUIState gameplayUIState, InteractableRaycaster interactableRaycaster)
		{
			_gameplayUIState = gameplayUIState.ThrowIfNull(nameof(gameplayUIState));
			_interactableRaycaster = interactableRaycaster.ThrowIfNull(nameof(interactableRaycaster));
			
			_messageBroker = Dependencies.Get<MessageBroker>();
			_gameplayInputState = new GameplayInputState();
			
			SubscriptionBuilder subscriptionBuilder = new(this);

			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<OpenUIRequest>()
				.SetCallback(OnOpenUIRequested)
				.Build());

			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<CurrentSelectedInteractable>()
				.SetCallback(OnPrimarySelectedInteractableReceived)
				.AddFilter(message => ((CurrentSelectedInteractable)message).InteractionType == InteractionType.PrimarySelection)
				.Build());
			
			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<CurrentSelectedInteractable>()
				.SetCallback(OnSecondarySelectedInteractableReceived)
				.AddFilter(message => ((CurrentSelectedInteractable)message).InteractionType == InteractionType.SecondarySelection)
				.Build());
		}

		public override bool AllowCameraInputs => true;
		public override bool AllowInteractions => true;

		protected override IInputState InputState => _gameplayInputState;
		protected override UIState UIState => _gameplayUIState;

		protected override void OnEnabled()
		{
			// Inputs
			_gameplayInputState.PauseInputPerformed += OnPauseInputPerformed;
			_gameplayInputState.CloseInfoWindowRequested += OnCloseInfoWindowRequested;
			
			// UI
			_gameplayUIState.PauseButtonPressed += OnPauseButtonPressed;
			_gameplayUIState.HotBarItemSelected += OnPlacementModeRequested;
			
			// Interactable Raycaster
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;

			_subscriptions.ForEach(sub => _messageBroker.Subscribe(sub));
		}

		protected override void OnDisabled()
		{
			// Inputs
			_gameplayInputState.PauseInputPerformed -= OnPauseInputPerformed;
			_gameplayInputState.CloseInfoWindowRequested -= OnCloseInfoWindowRequested;

			// UI
			_gameplayUIState.PauseButtonPressed -= OnPauseButtonPressed;
			_gameplayUIState.HotBarItemSelected -= OnPlacementModeRequested;
			
			// Interactable Raycaster
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;

			_subscriptions.ForEach(sub => _messageBroker.Unsubscribe(sub));
		}

		private void OnPauseInputPerformed(object sender, EventArgs args)
		{
			if (!_gameplayUIState.InfoWindow.IsOpen)
				PauseGameRequested?.Invoke(sender, args);
		}
		
		private void OnPauseButtonPressed(object sender, EventArgs args) => 
			PauseGameRequested?.Invoke(sender, args);
		
		private void OnPlacementModeRequested(object sender, WorldObjectDetails details) =>
			BeginPlacementModeRequested?.Invoke(this, new BeginPlacementModeEventArgs(details));

		private void OnCloseInfoWindowRequested(object sender, EventArgs args) =>
			_gameplayUIState.InfoWindow.CloseWindow();

		private void OnOpenUIRequested(IMessage message)
		{
			if (message is OpenUIRequest openUIRequest)
			{
				switch (openUIRequest.Window)
				{
					case UIWindow.TownHallWindow:
						OpenTownManagementWindow?.Invoke(this, EventArgs.Empty);
						break;
				}
			}
		}

		private void OnPrimarySelectedInteractableReceived(IMessage message)
		{
			if (message is CurrentSelectedInteractable selectedMessage)
			{
				if (selectedMessage.SelectedInteractable != null)
					_gameplayUIState.InfoWindow.OpenWindow(selectedMessage.SelectedInteractable);
				else
					_gameplayUIState.InfoWindow.CloseWindow();
			}
		}

		private void OnSecondarySelectedInteractableReceived(IMessage message)
		{
			if (message is CurrentSelectedInteractable selectedMessage)
			{
				if (selectedMessage.SelectedInteractable.TryGetComponent(out ContextMenuUser contextMenuUser))
				{
					_gameplayUIState.InfoWindow.OpenWindow(selectedMessage.SelectedInteractable);
					OpenContextMenuRequested?.Invoke(this, (contextMenuUser, Input.mousePosition));
				}
			}
		}
		
		private void OnNonInteractablePrimaryActionSelected(object sender, EventArgs args)
		{
			if (!UIState.IsMouseOverGameObject)
			{
				_gameplayUIState.InfoWindow.CloseWindow();
			}
		}
	}
}