using System;
using Game.Common;
using Game.Events;
using Game.Values;
using UnityEngine;

namespace Game
{
	[DisallowMultipleComponent]
	public class GameManager : MonoBehaviour
	{
		private bool _isGamePaused;
		private GameSpeed _gameSpeedBeforePause;

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
			_gameSpeedBeforePause = GameValues.Time.GameSpeed;
			
			//Time.timeScale = 0f;
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs(GameSpeed.Paused));
			GameEvents.General.GamePaused.Raise(this);
		}

		private void ResumeGame()
		{
			if (!_isGamePaused)
				return;
			
			_isGamePaused = false;
			//Time.timeScale = 1f;
			GameEvents.Time.ChangeGameSpeed.Request(this, new GameSpeedEventArgs(_gameSpeedBeforePause));
			GameEvents.General.GameResumed.Raise(this);
		}
		
		private void QuitGame()
		{
			// Todo: Save game
			// Todo: change to main menu scene
			Debug.Log("Quit Game not setup");
		}
	}
}