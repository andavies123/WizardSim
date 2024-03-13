using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIManagers
{
	public class PauseMenuUIManager : MonoBehaviour
	{
		public event Action ResumeButtonPressed;
		public event Action QuitButtonPressed;

		public void OnResumeButtonPressed() => ResumeButtonPressed?.Invoke();
		public void OnQuitButtonPressed() => QuitButtonPressed?.Invoke();
	}
}