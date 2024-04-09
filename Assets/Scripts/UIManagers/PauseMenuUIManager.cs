using System;

namespace UIManagers
{
	public class PauseMenuUIManager : UIManager
	{
		public event Action ResumeButtonPressed;
		public event Action QuitButtonPressed;

		public void OnResumeButtonPressed() => ResumeButtonPressed?.Invoke();
		public void OnQuitButtonPressed() => QuitButtonPressed?.Invoke();
	}
}