using System.Linq;
using UnityEngine;
using Utilities.Attributes;

namespace UI.MainMenu
{
	// Todo: Create a Load World page
	// Todo: Load World should display all saved worlds
	// Todo: Load World should be able to delete save files
	// Todo: Load World should show warning when deleting a world
	// Todo: Load World should show the name of the saved world
	// Todo: Load World should show details of the world to load
	// Todo: Load World should have a scrollable area
	// Todo: Load World should have a button to open the saves folder
	// Todo: Load World should have a refresh button to refresh the list of saves
	
	// Todo: Create Options page
	
	public class MainMenuUIManager : MonoBehaviour
	{
		[SerializeField, Required] private MainMenuUIPage startPage;
		
		private MainMenuUIPage _activePage;
		
		public void ChangePage(MainMenuUIPage newPage)
		{
			if (_activePage) _activePage.Disable();
			_activePage = newPage;
			if (_activePage) _activePage.Enable();
		}
		
		private void Start()
		{
			// Disable all pages that are children of this object
			foreach (MainMenuUIPage uiPage in GetComponentsInChildren<MainMenuUIPage>(true).ToList())
			{
				uiPage.gameObject.SetActive(true);
				uiPage.Disable();
			}
			
			// Enable the start page
			ChangePage(startPage);
		}
	}
}
