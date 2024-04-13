using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIManagers
{
	public class MainMenuUIManager : MonoBehaviour
	{
		[SerializeField] private string gameplaySceneName;
        
		[Header("UI Elements")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
	        playButton.onClick.AddListener(OnPlayButtonPressed);
	        quitButton.onClick.AddListener(OnQuitButtonPressed);
        }

        private void OnDestroy()
        {
	        playButton.onClick.RemoveListener(OnPlayButtonPressed);
	        quitButton.onClick.RemoveListener(OnQuitButtonPressed);
        }

        private void OnPlayButtonPressed() => SceneManager.LoadScene(gameplaySceneName);

		private void OnQuitButtonPressed()
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