using System;
using Extensions;
using Game.Events;

namespace Game.GameStates.PauseMenuStates
{
	public class PauseMenuGameState : GameState
	{
		private readonly PauseMenuUIState _pauseMenuUIState;
		private readonly PauseMenuInputState _pauseMenuInputState;

		public PauseMenuGameState(PauseMenuUIState pauseMenuUIState)
		{
			_pauseMenuUIState = pauseMenuUIState.ThrowIfNull(nameof(pauseMenuUIState));
			_pauseMenuInputState = new PauseMenuInputState();
		}

		public override bool AllowCameraInputs => false;
		public override bool AllowInteractions => false;

		protected override IInputState InputState => _pauseMenuInputState;
		protected override UIState UIState => _pauseMenuUIState;

		protected override void OnEnabled()
		{
			_pauseMenuInputState.ResumeActionPerformed += RequestResumeGame;
			_pauseMenuUIState.ResumeButtonPressed += RequestResumeGame;
			
			_pauseMenuUIState.QuitButtonPressed += RequestQuitGame;
		}

		protected override void OnDisabled()
		{
			_pauseMenuInputState.ResumeActionPerformed -= RequestResumeGame;
			_pauseMenuUIState.ResumeButtonPressed -= RequestResumeGame;

			_pauseMenuUIState.QuitButtonPressed -= RequestQuitGame;
		}

		private void RequestResumeGame(object sender, EventArgs args) => GameEvents.General.ResumeGame.Request(sender);
		private void RequestQuitGame(object sender, EventArgs args) => GameEvents.General.QuitGame.Request(sender);
	}
}