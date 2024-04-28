using System;
using TMPro;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuItemUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text itemName;

		private ContextMenuItem _contextMenuItem;
		
		public event Action ItemSelected;
		
		public void SetContextMenuItem(ContextMenuItem contextMenuItem)
		{
			_contextMenuItem = contextMenuItem;
			
			BuildUI();
		}

		public void OnContextMenuItemSelected()
		{
			if (_contextMenuItem != null)
			{
				ItemSelected?.Invoke();
				_contextMenuItem.MenuClickCallback?.Invoke();
			}
		}

		private void BuildUI()
		{
			if (_contextMenuItem == null)
				return;
			
			itemName.SetText(_contextMenuItem.Name);
		}
	}
}