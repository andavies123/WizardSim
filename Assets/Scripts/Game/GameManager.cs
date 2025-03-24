using System;
using UnityEngine;

namespace Game
{
	[DisallowMultipleComponent]
	public class GameManager : MonoBehaviour
	{
		private bool _isGamePaused;

		private void Awake()
		{
			GameEvents.General.PauseGame.Requested += OnPauseGameRequested;
			GameEvents.General.ResumeGame.Requested += OnResumeGameRequested;
			GameEvents.General.QuitGame.Requested += OnQuitGameRequested;
		}

		private void OnDestroy()
		{
			GameEvents.General.PauseGame.Requested -= OnPauseGameRequested;
			GameEvents.General.ResumeGame.Requested -= OnResumeGameRequested;
			GameEvents.General.QuitGame.Requested -= OnQuitGameRequested;
		}

		private void OnPauseGameRequested(object sender, EventArgs _) => PauseGame();
		private void OnResumeGameRequested(object sender, EventArgs _) => ResumeGame();
		private void OnQuitGameRequested(object sender, EventArgs _) => QuitGame();

		private void PauseGame()
		{
			if (_isGamePaused)
				return;
			
			_isGamePaused = true;
			Time.timeScale = 0f;
			GameEvents.General.PauseGame.Activate(new GameEventArgs(this));
		}

		private void ResumeGame()
		{
			if (!_isGamePaused)
				return;
			
			_isGamePaused = false;
			Time.timeScale = 1f;
			GameEvents.General.ResumeGame.Activate(new GameEventArgs(this));
		}
		
		private void QuitGame()
		{
			// Todo: Save game
			// Todo: change to main menu scene
		}
	}
}