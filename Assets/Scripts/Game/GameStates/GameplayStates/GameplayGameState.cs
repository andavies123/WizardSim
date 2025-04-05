using System;
using Extensions;
using Game.Events;
using GameWorld.WorldObjects;
using UI;
using UI.ContextMenus;

namespace Game.GameStates.GameplayStates
{
	public class GameplayGameState : GameState
	{
		private readonly GameplayUIState _gameplayUIState;
		private readonly GameplayInputState _gameplayInputState;

		public event EventHandler PauseGameRequested;
		public event Action<IContextMenuUser[]> OpenContextMenuRequested;
		public event EventHandler<BeginPlacementModeEventArgs> BeginPlacementModeRequested;
		public event EventHandler OpenTownManagementWindow;
		public event EventHandler OpenTaskManagementWindow
		{
			add => _gameplayInputState.OpenTaskManagementRequested += value;
			remove => _gameplayInputState.OpenTaskManagementRequested -= value;
		}

		public GameplayGameState(GameplayUIState gameplayUIState)
		{
			_gameplayUIState = gameplayUIState.ThrowIfNull(nameof(gameplayUIState));
			_gameplayInputState = new GameplayInputState();
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

			GameEvents.Interaction.CurrentSelectedInteractableUpdated.Raised += OnCurrentSelectedInteractableUpdated;
			GameEvents.UI.OpenUI.Requested += OnOpenUIRequested;
		}

		protected override void OnDisabled()
		{
			// Inputs
			_gameplayInputState.PauseInputPerformed -= OnPauseInputPerformed;
			_gameplayInputState.CloseInfoWindowRequested -= OnCloseInfoWindowRequested;

			// UI
			_gameplayUIState.PauseButtonPressed -= OnPauseButtonPressed;
			_gameplayUIState.HotBarItemSelected -= OnPlacementModeRequested;
			
			GameEvents.Interaction.CurrentSelectedInteractableUpdated.Raised -= OnCurrentSelectedInteractableUpdated;
			GameEvents.UI.OpenUI.Requested -= OnOpenUIRequested;
		}

		private void OnPauseInputPerformed(object sender, EventArgs args)
		{
			if (!_gameplayUIState.InfoWindow.IsOpen)
				PauseGameRequested?.Invoke(sender, args);
		}
		
		private void OnPauseButtonPressed(object sender, EventArgs args) => 
			PauseGameRequested?.Invoke(sender, args);
		
		private void OnPlacementModeRequested(object sender, WorldObjectDetails details) =>
			BeginPlacementModeRequested?.Invoke(this, new BeginPlacementModeEventArgs {WorldObjectDetails = details});

		private void OnCloseInfoWindowRequested(object sender, EventArgs args) =>
			_gameplayUIState.InfoWindow.CloseWindow();

		private void OnOpenUIRequested(object sender, OpenUIEventArgs args)
		{
			switch (args.Window)
			{ 
				case UIWindow.TownHallWindow:
					OpenTownManagementWindow?.Invoke(this, EventArgs.Empty);
					break;
				case UIWindow.UpgradeWindow:
				default:
					break;
			}
		}

		private void OnCurrentSelectedInteractableUpdated(object sender, SelectedInteractableEventArgs args)
		{
			switch (args.SelectionType)
			{
				case SelectionType.PrimarySelection:
					HandlePrimarySelectedInteractableReceived(args.SelectedInteractable);
					break;
				case SelectionType.SecondarySelection:
					HandleSecondarySelectedInteractableReceived(args.SelectedInteractable);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(args.SelectionType));
			}
		}

		private void HandlePrimarySelectedInteractableReceived(Interactable selectedInteractable)
		{
			if (selectedInteractable)
				_gameplayUIState.InfoWindow.OpenWindow(selectedInteractable);
			else
				_gameplayUIState.InfoWindow.CloseWindow();
		}

		private void HandleSecondarySelectedInteractableReceived(Interactable selectedInteractable)
		{
			if (selectedInteractable)
			{
				IContextMenuUser[] contextMenuUsers = selectedInteractable.GetComponents<IContextMenuUser>();

				if (contextMenuUsers.Length > 0)
				{
					_gameplayUIState.InfoWindow.OpenWindow(selectedInteractable);
					OpenContextMenuRequested?.Invoke(contextMenuUsers);
				}
			}
		}
	}
}