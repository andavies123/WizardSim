using System;
using Extensions;
using UnityEngine;

namespace Game.GameStates.PauseMenuStates
{
	public class PauseMenuGameState : GameState
	{
		private readonly PauseMenuUIState _pauseMenuUIState;
		private readonly PauseMenuInputState _pauseMenuInputState;

		public event EventHandler ResumeGameRequested 
		{
			add 
			{
				_pauseMenuInputState.ResumeActionPerformed += value;
				_pauseMenuUIState.ResumeButtonPressed += value;
			}
			remove 
			{
				_pauseMenuInputState.ResumeActionPerformed -= value;
				_pauseMenuUIState.ResumeButtonPressed -= value;
			}
		}
		
		public event EventHandler QuitGameRequested
		{
			add => _pauseMenuUIState.QuitButtonPressed += value;
			remove => _pauseMenuUIState.QuitButtonPressed -= value;
		}
        
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
			GameManager.Instance.PauseGame();
		}

		protected override void OnDisabled()
		{
			GameManager.Instance.ResumeGame();
		}
	}
}