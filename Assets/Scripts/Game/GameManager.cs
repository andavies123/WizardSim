using System;
using InputStates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private GameSceneInputManager inputManager;
		
		[Header("Scene Names")]
		[SerializeField] private string mainMenuSceneName;
		
		private bool _isGamePaused = false;

		public event Action GamePaused;
		public event Action GameResumed;

		public void PauseGame()
		{
			if (_isGamePaused)
				return;
			
			_isGamePaused = true;
			Time.timeScale = 0f;
			GamePaused?.Invoke();
		}

		public void ResumeGame()
		{
			if (!_isGamePaused)
				return;
			
			_isGamePaused = false;
			Time.timeScale = 1f;
			GameResumed?.Invoke();
		}

		public void QuitGame()
		{
			// Todo: Add a save game call here
			SceneManager.LoadScene(mainMenuSceneName);
		}
		
		private void Start()
		{
			inputManager.GameplayInputState.PauseActionPerformed += PauseGame;
			inputManager.PauseMenuInputState.ResumeActionPerformed += ResumeGame;
		}

		private void OnDestroy()
		{
			inputManager.GameplayInputState.PauseActionPerformed -= PauseGame;
			inputManager.PauseMenuInputState.ResumeActionPerformed -= ResumeGame;
		}
	}
}