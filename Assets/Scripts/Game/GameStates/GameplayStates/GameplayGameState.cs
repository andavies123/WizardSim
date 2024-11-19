﻿using System;
using System.Collections.Generic;
using CameraComponents;
using Extensions;
using GameWorld.WorldObjects;
using Messages.UI;
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
		private readonly List<ISubscription> _subscriptions = new();
		private MessageBroker _messageBroker;

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
            
			_gameplayInputState = new GameplayInputState(interactableRaycaster);
		}

		public override bool AllowCameraInputs => true;
		public override bool AllowInteractions => true;

		protected override IInputState InputState => _gameplayInputState;
		protected override UIState UIState => _gameplayUIState;

		protected override void OnEnabled()
		{
			// Inputs
			_gameplayInputState.PauseInputPerformed += OnPauseInputPerformed;
			_gameplayInputState.OpenContextMenuRequested += OnOpenContextMenuRequested;
			_gameplayInputState.OpenInfoWindowRequested += OnOpenInfoWindowRequested;
			_gameplayInputState.CloseInfoWindowRequested += OnCloseInfoWindowRequested;
			
			// UI
			_gameplayUIState.PauseButtonPressed += OnPauseButtonPressed;
			_gameplayUIState.HotBarItemSelected += OnPlacementModeRequested;

			SubscriptionBuilder subscriptionBuilder = new(this);

			_subscriptions.Add(subscriptionBuilder
				.ResetAllButSubscriber()
				.SetMessageType<OpenUIRequest>()
				.SetCallback(OnOpenUIRequested)
				.Build());
		}

		protected override void OnDisabled()
		{
			// Inputs
			_gameplayInputState.PauseInputPerformed -= OnPauseInputPerformed;
			_gameplayInputState.OpenContextMenuRequested -= OnOpenContextMenuRequested;
			_gameplayInputState.OpenInfoWindowRequested -= OnOpenInfoWindowRequested;
			_gameplayInputState.CloseInfoWindowRequested -= OnCloseInfoWindowRequested;

			// UI
			_gameplayUIState.PauseButtonPressed -= OnPauseButtonPressed;
			_gameplayUIState.HotBarItemSelected -= OnPlacementModeRequested;
		}

		private void OnPauseInputPerformed(object sender, EventArgs args)
		{
			if (!_gameplayUIState.InfoWindow.IsOpen)
				PauseGameRequested?.Invoke(sender, args);
		}
		
		private void OnPauseButtonPressed(object sender, EventArgs args) => 
			PauseGameRequested?.Invoke(sender, args);

		private void OnOpenContextMenuRequested(object sender, ContextMenuUser contextMenuUser) => 
			OpenContextMenuRequested?.Invoke(sender, (contextMenuUser, Input.mousePosition));
		
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
				Debug.Log("Open UI Requested");
			}
		}
	}
}