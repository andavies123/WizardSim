using UI.MainMenu;
using UnityEngine;
using Utilities.Attributes;

namespace SceneFlowManagers
{
	/// <summary>
	/// Class to handle the flow of the main menu scene.
	///
	/// Once this scene is loaded, this class will be in charge of making
	/// sure the right series of events occurs
	///
	/// Since not much happens in the main menu, this shouldn't be too large (for now)
	/// </summary>
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