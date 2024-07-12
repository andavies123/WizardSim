using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ContextMenus
{
	[RequireComponent(typeof(RectTransform))]
	public class ContextMenuItemGroupUI : MonoBehaviour
	{
		[SerializeField] private ContextMenuItemUI contextMenuItemPrefab;
		
		private readonly List<ContextMenuItemUI> _menuItemUIs = new();
		private readonly ContextMenuItem _backMenuItem = new("Back")
		{
			IsEnabledFunc = ContextMenuItem.AlwaysTrue,
			IsVisibleFunc = ContextMenuItem.AlwaysTrue,
			MenuClickCallback = null,
			IsBack = true
		};
		private ContextMenuItemUI _selectedMenuItemUI = null;

		public event Action BackItemSelected;
		public event Action<ContextMenuItemUI> LeafItemSelected;
		public event Action<ContextMenuItemUI> GroupItemSelected;
		
		public RectTransform RectTransform { get; private set; }

		public void Initialize(List<ContextMenuItem> menuItems, int treeIndex)
		{
			CleanUp();

			// Don't want to add the back option to the first group
			if (treeIndex > 1)
			{
				_menuItemUIs.Add(CreateMenuItemUI(_backMenuItem, treeIndex));
			}
			
			// Loop through the rest
			foreach (ContextMenuItem menuItem in menuItems)
			{
				_menuItemUIs.Add(CreateMenuItemUI(menuItem, treeIndex));
			}
		}
        
		public void CleanUp()
		{
			foreach (ContextMenuItemUI menuItemUI in _menuItemUIs)
			{
				menuItemUI.MenuItemSelected -= OnContextMenuItemSelected;
				Destroy(menuItemUI.gameObject);
			}
			
			_menuItemUIs.Clear();
		}

		private ContextMenuItemUI CreateMenuItemUI(ContextMenuItem menuItem, int treeIndex)
		{
			ContextMenuItemUI menuItemUI = Instantiate(contextMenuItemPrefab, transform);
			menuItemUI.Initialize(menuItem, treeIndex);
			menuItemUI.MenuItemSelected += OnContextMenuItemSelected;
			return menuItemUI;
		}

		private void OnContextMenuItemSelected(ContextMenuItemUI menuItemUI)
		{
			if (menuItemUI.ContextMenuItem == _backMenuItem)
			{
				BackItemSelected?.Invoke();
			}
			else if (menuItemUI.ContextMenuItem.IsLeaf)
			{
				LeafItemSelected?.Invoke(menuItemUI);
			}
			else
			{
				_selectedMenuItemUI = menuItemUI;
				GroupItemSelected?.Invoke(menuItemUI);
			}
		}

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
		}
	}
}