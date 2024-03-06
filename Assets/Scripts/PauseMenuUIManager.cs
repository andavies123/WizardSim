using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUIManager : MonoBehaviour
{
	[SerializeField] private string mainMenuSceneName;

	public event Action ResumeButtonPressed; 

	public void OnResumeButtonPressed()
	{
		Time.timeScale = 1f;
		ResumeButtonPressed?.Invoke();
	}

	public void OnQuitButtonPressed()
	{
		SceneManager.LoadScene(mainMenuSceneName);
	}
}