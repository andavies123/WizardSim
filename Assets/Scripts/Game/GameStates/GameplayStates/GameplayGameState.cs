using System;
using CameraComponents;
using Extensions;
using Game.Messages;
using Game.MessengerSystem;
using InputStates.InputEventArgs;

namespace Game.GameStates.GameplayStates
{
	public class GameplayGameState : GameState
	{
		private readonly GameplayUIState _gameplayUIState;
		private readonly GameplayInputState _gameplayInputState;

		public event EventHandler PauseGameRequested;
		public event EventHandler<OpenContextMenuEventArgs> OpenContextMenuRequested;
		public event EventHandler<BeginPlacementModeEventArgs> BeginPlacementModeRequested;
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
			
			// UI
			_gameplayUIState.PauseButtonPressed += OnPauseButtonPressed;
			
			// Global Messenger
			GlobalMessenger.Subscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequested);
		}

		protected override void OnDisabled()
		{
			// Inputs
			_gameplayInputState.PauseInputPerformed -= OnPauseInputPerformed;
			_gameplayInputState.OpenContextMenuRequested -= OnOpenContextMenuRequested;
			
			// UI
			_gameplayUIState.PauseButtonPressed -= OnPauseButtonPressed;
			
			// Global Messenger
			GlobalMessenger.Unsubscribe<BeginPlacementModeRequest>(OnBeginPlacementModeRequested);
		}

		private void OnPauseInputPerformed(object sender, EventArgs args) => 
			PauseGameRequested?.Invoke(sender, args);
		
		private void OnPauseButtonPressed(object sender, EventArgs args) => 
			PauseGameRequested?.Invoke(sender, args);

		private void OnOpenContextMenuRequested(object sender, OpenContextMenuEventArgs args) => 
			OpenContextMenuRequested?.Invoke(sender, args);

		private void OnBeginPlacementModeRequested(BeginPlacementModeRequest request) =>
			BeginPlacementModeRequested?.Invoke(this, new BeginPlacementModeEventArgs(request.PlacementDetails));
	}
}