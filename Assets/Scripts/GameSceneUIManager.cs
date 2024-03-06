using UnityEngine;

public class GameSceneUIManager : MonoBehaviour
{
	[SerializeField] private PlayMenuUIManager playMenuUIManager;
	[SerializeField] private PauseMenuUIManager pauseMenuUIManager;

	private void Start()
	{
		playMenuUIManager.PauseButtonPressed += OnPauseButtonPressed;
		pauseMenuUIManager.ResumeButtonPressed += OnResumeButtonPressed;
		
		playMenuUIManager.gameObject.SetActive(true);
		pauseMenuUIManager.gameObject.SetActive(false);
	}
	
	private void OnPauseButtonPressed()
	{
		playMenuUIManager.gameObject.SetActive(false);
		pauseMenuUIManager.gameObject.SetActive(true);
	}

	private void OnResumeButtonPressed()
	{
		playMenuUIManager.gameObject.SetActive(true);
		pauseMenuUIManager.gameObject.SetActive(false);
	}
}