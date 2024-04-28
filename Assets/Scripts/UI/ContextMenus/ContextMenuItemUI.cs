using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ContextMenus
{
	public class ContextMenuItemUI : MonoBehaviour
	{
		[SerializeField] private Button button;
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

			button.interactable = _contextMenuItem.IsEnabled;
		}

		private void Awake()
		{
			button.onClick.AddListener(OnContextMenuItemSelected);
		}

		private void OnDestroy()
		{
			button.onClick.RemoveListener(OnContextMenuItemSelected);
		}
	}
}