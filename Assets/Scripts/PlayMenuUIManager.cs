using System;
using UnityEngine;

public class PlayMenuUIManager : MonoBehaviour
{
	public event Action PauseButtonPressed;
	
	public void OnPauseButtonPressed()
	{
		Time.timeScale = 0f;
		PauseButtonPressed?.Invoke();
	}
}