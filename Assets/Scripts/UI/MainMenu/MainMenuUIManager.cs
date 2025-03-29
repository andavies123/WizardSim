using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.MainMenu
{
	[DisallowMultipleComponent]
	public class MainMenuUIManager : MonoBehaviour
	{
		private List<MainMenuUIPage> _allMainMenuPages = new();
		private MainMenuUIPage _activePage;
		
		public void SetActivePage(MainMenuUIPage newPage)
		{
			if (_activePage)
				_activePage.Disable();
			
			_activePage = newPage;
			
			if (_activePage)
				_activePage.Enable();
		}

		public void DisableAllPages()
		{
			_allMainMenuPages.ForEach(page => page.Disable());
		}
		
		private void Awake()
		{
			_allMainMenuPages = GetComponentsInChildren<MainMenuUIPage>(true).ToList();
		}
	}
}
