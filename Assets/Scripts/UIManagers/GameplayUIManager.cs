using System;
using UnityEngine;

namespace UIManagers
{
	public class GameplayUIManager : MonoBehaviour
	{
		public event Action PauseButtonPressed;
	
		public void OnPauseButtonPressed() => PauseButtonPressed?.Invoke();
	}	
}