using System;
using InputStates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	[DisallowMultipleComponent]
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private GameSceneInputManager inputManager;
		
		[Header("Scene Names")]
		[SerializeField] private string mainMenuSceneName;
		
		private bool _isGamePaused = false;

		public event EventHandler GamePaused;
		public event EventHandler GameResumed;

		public static GameManager Instance { get; private set; }
		
		public void PauseGame()
		{
			if (_isGamePaused)
				return;
			
			_isGamePaused = true;
			Time.timeScale = 0f;
			GamePaused?.Invoke(this, EventArgs.Empty);
		}

		public void ResumeGame()
		{
			if (!_isGamePaused)
				return;
			
			_isGamePaused = false;
			Time.timeScale = 1f;
			GameResumed?.Invoke(this, EventArgs.Empty);
		}

		public void QuitGame()
		{
			// Todo: Add a save game call here
			SceneManager.LoadScene(mainMenuSceneName);
		}

		private void Awake()
		{
			if (Instance)
			{
				Debug.Log($"Unable to have multiple {nameof(GameManager)}. Deleting this instance.", gameObject);
				Destroy(this);
				return;
			}

			Instance = this;
			
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