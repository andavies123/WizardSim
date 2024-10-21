using System.Linq;
using UnityEngine;
using Utilities.Attributes;

namespace UI.MainMenu
{
	// Todo: Create a Load World page
	// Todo: Create Options page
	[DisallowMultipleComponent]
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
