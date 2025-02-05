using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	[DisallowMultipleComponent]
	public class GameManager : MonoBehaviour
	{
		private bool _isGamePaused = false;

		public static GameManager Instance { get; private set; }
		
		public void PauseGame()
		{
			if (_isGamePaused)
				return;
			
			_isGamePaused = true;
			Time.timeScale = 0f;
		}

		public void ResumeGame()
		{
			if (!_isGamePaused)
				return;
			
			_isGamePaused = false;
			Time.timeScale = 1f;
		}

		public void QuitGame()
		{
			// Todo: Add a save game call here
			SceneManager.LoadScene("Scenes/MainMenuScene");
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
		}
	}
}