using System;

namespace UIManagers
{
	public class GameplayUIManager : UIManager
	{
		public event Action PauseButtonPressed;
	
		public void OnPauseButtonPressed() => PauseButtonPressed?.Invoke();
	}
}