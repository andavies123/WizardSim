using System;
using System.Collections.Generic;
using CameraComponents;
using Codice.CM.Client.Differences.Merge;
using Extensions;
using GameWorld.WorldObjects;
using Messages.Selection;
using Messages.UI;
using Messages.UI.Enums;
using MessagingSystem;
using UI;
using UI.ContextMenus;
using UI.Messages;
using UnityEngine;

namespace Game.GameStates.GameplayStates
{
	public class GameplayGameState : GameState
	{
		private readonly GameplayUIState _gameplayUIState;
		private readonly GameplayInputState _gameplayInputState;
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
			interactableRaycaster.ThrowIfNull(nameof(interactableRaycaster));
			_messageBroker = Dependencies.Get<MessageBroker>();
            
			_gameplayInputState = new GameplayInputState(interactableRaycaster);
			
			SubscriptionBuilder subscriptionBuilder = new(this);

			// Todo: This keeps getting unsubscribed everytime the context menu opens
			// I believe I shouldn't be unsubscribing in on disabled and instead check if the state is active
			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<OpenUIRequest>()
				.SetCallback(OnOpenUIRequested)
				.Build());

			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<CurrentSelectedInteractable>()
				.SetCallback(OnPrimarySelectedInteractableReceived)
				.AddFilter(message => (message as CurrentSelectedInteractable).InteractionType == InteractionType.PrimarySelection)
				.Build());

			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<OpenContextMenuRequest>()
				.SetCallback(OnOpenContextMenuRequestReceived)
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
			_gameplayInputState.OpenInfoWindowRequested += OnOpenInfoWindowRequested;
			_gameplayInputState.CloseInfoWindowRequested += OnCloseInfoWindowRequested;
			
			// UI
			_gameplayUIState.PauseButtonPressed += OnPauseButtonPressed;
			_gameplayUIState.HotBarItemSelected += OnPlacementModeRequested;

			_subscriptions.ForEach(sub => _messageBroker.Subscribe(sub));
		}

		protected override void OnDisabled()
		{
			// Inputs
			_gameplayInputState.PauseInputPerformed -= OnPauseInputPerformed;
			_gameplayInputState.OpenInfoWindowRequested -= OnOpenInfoWindowRequested;
			_gameplayInputState.CloseInfoWindowRequested -= OnCloseInfoWindowRequested;

			// UI
			_gameplayUIState.PauseButtonPressed -= OnPauseButtonPressed;
			_gameplayUIState.HotBarItemSelected -= OnPlacementModeRequested;

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

		private void OnOpenInfoWindowRequested(object sender, Interactable interactable) =>
			_gameplayUIState.InfoWindow.OpenWindow(interactable);

		private void OnCloseInfoWindowRequested(object sender, EventArgs args) =>
			_gameplayUIState.InfoWindow.CloseWindow();

		private void OnOpenUIRequested(IMessage message)
		{
			if (message is OpenUIRequest openUIRequest)
			{
				if (openUIRequest.Window == UIWindow.TownHallWindow)
				{
					OpenTownManagementWindow?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private void OnPrimarySelectedInteractableReceived(IMessage message)
		{
			if (message is CurrentSelectedInteractable selectedInteractableMessage)
			{
				// Close context menu first
				_gameplayUIState.InfoWindow.OpenWindow(selectedInteractableMessage.SelectedInteractable);
			}
		}

		private void OnOpenContextMenuRequestReceived(IMessage message)
		{
			if (message is OpenContextMenuRequest request)
			{
				OpenContextMenuRequested?.Invoke(this, (request.ContextMenuUser, request.ScreenPosition));
			}
		}
	}
}