using System;
using InputStates;
using UIStates;

namespace Game
{
	public class PauseGameState : IGameState
	{
		private readonly InputStateMachine _inputStateMachine;
		private readonly PauseMenuInput _pauseMenuInput;
		private readonly PauseMenuUIState _pauseMenuUIState;
		
		public event EventHandler ResumeGameRequested;
		public event EventHandler QuitGameRequested;
        
		public PauseGameState(PauseMenuUIState pauseMenuUIState)
		{
			_pauseMenuUIState = pauseMenuUIState;
			_pauseMenuInput = new PauseMenuInput();
			
			Dependencies.RegisterDependency(_pauseMenuInput);
		}

		public void Enable()
		{
			if (_pauseMenuInput != null)
			{
				_pauseMenuInput.ResumeActionPerformed += OnPauseMenuInputResumeActionPerformed;
				_pauseMenuInput?.Enable();
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
			if (_pauseMenuInput != null)
			{
				_pauseMenuInput.Disable();
				_pauseMenuInput.ResumeActionPerformed -= OnPauseMenuInputResumeActionPerformed;
			}

			if (_pauseMenuUIState)
			{
				_pauseMenuUIState.Disable();
				_pauseMenuUIState.ResumeButtonPressed -= OnPauseMenuUIResumeButtonPressed;
				_pauseMenuUIState.QuitButtonPressed -= OnPauseMenuUIQuitButtonPressed;
			}
		}

		private void OnPauseMenuInputResumeActionPerformed(object sender, EventArgs args) => RaiseResumeGameRequested(sender);
		private void OnPauseMenuUIResumeButtonPressed(object sender, EventArgs args) => RaiseResumeGameRequested(sender);
		private void OnPauseMenuUIQuitButtonPressed(object sender, EventArgs args) => RaiseQuitGameRequested(sender);
		
		private void RaiseResumeGameRequested(object sender) => ResumeGameRequested?.Invoke(sender, EventArgs.Empty);
		private void RaiseQuitGameRequested(object sender) => QuitGameRequested?.Invoke(sender, EventArgs.Empty);
	}
}