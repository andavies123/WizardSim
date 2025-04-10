using System;
using Extensions;
using Game.Events;
using GameWorld.WorldObjects;
using UI.ContextMenus;

namespace Game.GameStates.GameplayStates
{
	public class GameplayGameState : GameState
	{
		private readonly GameplayUIState _gameplayUIState;
		private readonly GameplayInputState _gameplayInputState = new();
		private readonly SelectionManager _selectionManager;

		public event EventHandler PauseGameRequested;
		public event Action<IContextMenuUser[]> OpenContextMenuRequested;
		public event EventHandler<BeginPlacementModeEventArgs> BeginPlacementModeRequested;
		public event EventHandler OpenTownManagementWindow;
		public event EventHandler OpenTaskManagementWindow
		{
			add => _gameplayInputState.OpenTaskManagementRequested += value;
			remove => _gameplayInputState.OpenTaskManagementRequested -= value;
		}

		public GameplayGameState(GameplayUIState gameplayUIState, SelectionManager selectionManager)
		{
			_gameplayUIState = gameplayUIState.ThrowIfNull(nameof(gameplayUIState));
			_selectionManager = selectionManager.ThrowIfNull(nameof(selectionManager));
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

			_selectionManager.PrimarySelectionUpdated += OnPrimarySelectionUpdated;
			_selectionManager.SecondarySelectionUpdated += OnSecondarySelectionUpdated;

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

			_selectionManager.PrimarySelectionUpdated += OnPrimarySelectionUpdated;
			_selectionManager.SecondarySelectionUpdated += OnSecondarySelectionUpdated;
			
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

		private void OnPrimarySelectionUpdated(object sender, SelectionUpdatedEventArgs args)
		{
			if (args.Selection)
				_gameplayUIState.InfoWindow.OpenWindow(args.Selection);
			else
				_gameplayUIState.InfoWindow.CloseWindow();
		}

		private void OnSecondarySelectionUpdated(object sender, SelectionUpdatedEventArgs args)
		{
			if (args.Selection)
			{
				IContextMenuUser[] contextMenuUsers = args.Selection.GetComponents<IContextMenuUser>();

				if (contextMenuUsers.Length > 0)
				{
					_gameplayUIState.InfoWindow.OpenWindow(args.Selection);
					OpenContextMenuRequested?.Invoke(contextMenuUsers);
				}
			}
		}
	}
}