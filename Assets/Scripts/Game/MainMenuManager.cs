using UIStates;
using UnityEngine;

namespace Game
{
	public class MainMenuManager : MonoBehaviour
	{
		[Header("UI States")]
		[SerializeField] private MainMenuUIState mainMenuUIState;

		private void Start()
		{
			mainMenuUIState.Enable();
		}
	}
}