using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagers
{
	public class PauseMenuUIManager : UIManager
	{
		[Header("UI Elements")]
		[SerializeField] private Button resumeButton;
		[SerializeField] private Button quitButton;
		
		public event Action ResumeButtonPressed;
		public event Action QuitButtonPressed;

		protected override void Awake()
		{
			base.Awake();
			
			resumeButton.onClick.AddListener(OnResumeButtonPressed);
			quitButton.onClick.AddListener(OnQuitButtonPressed);
		}
		
		private void OnResumeButtonPressed() => ResumeButtonPressed?.Invoke();
		private void OnQuitButtonPressed() => QuitButtonPressed?.Invoke();
	}
}