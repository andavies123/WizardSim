using System.Linq;
using UnityEngine;
using Utilities.Attributes;

namespace UI.MainMenu
{
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
			foreach (MainMenuUIPage uiPage in GetComponentsInChildren<MainMenuUIPage>().ToList())
			{
				uiPage.Disable();
			}
			
			// Enable the start page
			ChangePage(startPage);
		}
	}
}
