using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIManagers
{
	public class MainMenuUIManager : MonoBehaviour
	{
		[SerializeField] private string gameplaySceneName;
	
		public void OnPlayButtonPressed()
		{
			SceneManager.LoadScene(gameplaySceneName);
		}

		public void OnQuitButtonPressed()
		{
#if UNITY_STANDALONE
			Application.Quit();
#endif
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}
}