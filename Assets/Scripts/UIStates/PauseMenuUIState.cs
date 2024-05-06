using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIStates
{
	public class PauseMenuUIState : UIState
	{
		[Header("UI Elements")]
		[SerializeField] private Button resumeButton;
		[SerializeField] private Button quitButton;
		
		public event EventHandler ResumeButtonPressed;
		public event EventHandler QuitButtonPressed;

		protected override void OnStateEnabled()
		{
			resumeButton.onClick.AddListener(OnResumeButtonPressed);
			quitButton.onClick.AddListener(OnQuitButtonPressed);
		}

		protected override void OnStateDisabled()
		{
			resumeButton.onClick.RemoveListener(OnResumeButtonPressed);
			quitButton.onClick.RemoveListener(OnQuitButtonPressed);
		}

		private void OnResumeButtonPressed() => ResumeButtonPressed?.Invoke(this, EventArgs.Empty);
		private void OnQuitButtonPressed() => QuitButtonPressed?.Invoke(this, EventArgs.Empty);
	}
}