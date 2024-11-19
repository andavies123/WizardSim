using System;
using Game.GameStates.PauseMenuStates;
using InputStates;

namespace Game
{
	public class PauseGameState : IGameState
	{
		private readonly InputStateMachine _inputStateMachine;
		private readonly PauseMenuInputState pauseMenuInputState;
		private readonly PauseMenuUIState _pauseMenuUIState;
		
		public event EventHandler ResumeGameRequested;
		public event EventHandler QuitGameRequested;
        
		public PauseGameState(PauseMenuUIState pauseMenuUIState)
		{
			_pauseMenuUIState = pauseMenuUIState;
			pauseMenuInputState = new PauseMenuInputState();
			
			Dependencies.Register(pauseMenuInputState);
		}

		public void Enable()
		{
			if (pauseMenuInputState != null)
			{
				pauseMenuInputState.ResumeActionPerformed += OnPauseMenuInputStateResumeActionPerformed;
				pauseMenuInputState?.Enable();
			}

			if (_pauseMenuUIState)
			{
				_pauseMenuUIState.ResumeButtonPressed += OnPauseMenuUIResumeButtonPressed;
				_pauseMenuUIState.QuitButtonPressed += OnPauseMenuUIQuitButtonPressed;
				_pauseMenuUIState.Enable();
			}
		}

		public void Disable()
		{
			if (pauseMenuInputState != null)
			{
				pauseMenuInputState.Disable();
				pauseMenuInputState.ResumeActionPerformed -= OnPauseMenuInputStateResumeActionPerformed;
			}

			if (_pauseMenuUIState)
			{
				_pauseMenuUIState.Disable();
				_pauseMenuUIState.ResumeButtonPressed -= OnPauseMenuUIResumeButtonPressed;
				_pauseMenuUIState.QuitButtonPressed -= OnPauseMenuUIQuitButtonPressed;
			}
		}

		private void OnPauseMenuInputStateResumeActionPerformed(object sender, EventArgs args) => RaiseResumeGameRequested(sender);
		private void OnPauseMenuUIResumeButtonPressed(object sender, EventArgs args) => RaiseResumeGameRequested(sender);
		private void OnPauseMenuUIQuitButtonPressed(object sender, EventArgs args) => RaiseQuitGameRequested(sender);
		
		private void RaiseResumeGameRequested(object sender) => ResumeGameRequested?.Invoke(sender, EventArgs.Empty);
		private void RaiseQuitGameRequested(object sender) => QuitGameRequested?.Invoke(sender, EventArgs.Empty);
	}
}