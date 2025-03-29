using UI.MainMenu;
using UnityEngine;
using Utilities.Attributes;

namespace SceneFlowManagers
{
	[DisallowMultipleComponent]
	public class MainMenuSceneFlowManager : MonoBehaviour
	{
		[SerializeField, Required] private MainMenuUIManager mainMenuUIManager;
		[SerializeField, Required] private MainMenuUIPage startPage;
		
		private void Start()
		{
			// Show the correct UI
			mainMenuUIManager.DisableAllPages();
			mainMenuUIManager.SetActivePage(startPage);
		}
	}
}