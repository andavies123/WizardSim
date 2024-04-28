using System;
using System.Collections.Generic;
using System.Linq;
using GameObjectPools;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenu : MonoBehaviour
	{
		[SerializeField] private GameObject contextMenuItemPrefab;
		[SerializeField] private Transform contextMenuItemParent;
		[SerializeField] private Transform inactiveMenuItemContainer;
		
		private readonly List<ContextMenuItemUI> _contextMenuItemUIs = new();
		private GameObjectPool _menuItemPool;
		private ContextMenuUser _contextMenuUser;

		public event EventHandler MenuClosed;

		public bool IsOpen { get; private set; }

		public void OpenMenu(ContextMenuUser user)
		{
			if (_contextMenuUser)
				_contextMenuUser.CloseMenu();

			_contextMenuUser = user;
			BuildContextMenu();
			gameObject.SetActive(true);
			IsOpen = true;
		}
		
		public void CloseMenu()
		{
			IsOpen = false;
			gameObject.SetActive(false);
			
			if (_contextMenuUser)
			{
				_contextMenuUser.CloseMenu();
				_contextMenuUser = null;
			}
			
			foreach (ContextMenuItemUI menuItemUI in _contextMenuItemUIs)
				_menuItemPool.ReleaseToPool(menuItemUI.gameObject);
			_contextMenuItemUIs.Clear();
			
			MenuClosed?.Invoke(this, EventArgs.Empty);
		}

		private void Awake()
		{
			_menuItemPool = new GameObjectPool(
				contextMenuItemPrefab,
				inactiveMenuItemContainer,
				5, 20);
		}

		private void BuildContextMenu()
		{
			if (!_contextMenuUser || _contextMenuUser.AllMenuItems.Count == 0)
				return;

			SynchronizeContextMenuItemCollectionLengths();
			BuildContextMenuItems();
		}

		private void SynchronizeContextMenuItemCollectionLengths()
		{
			_contextMenuUser.UpdateMenuItems();
			int visibleMenuItems = _contextMenuUser.AllMenuItems.Count(item => item.IsVisible);
			
			if (_contextMenuItemUIs.Count < visibleMenuItems)
			{
				while (_contextMenuItemUIs.Count < visibleMenuItems)
				{
					AddMenuItemToEnd();
				}
			}
			else if (_contextMenuItemUIs.Count > visibleMenuItems)
			{
				while (_contextMenuItemUIs.Count > visibleMenuItems)
				{
					RemoveLastMenuItem();
				}
			}
		}

		private void AddMenuItemToEnd()
		{
			if (!_menuItemPool.GetFromPool(contextMenuItemParent).TryGetComponent(out ContextMenuItemUI menuItemUI))
				return;
			
			menuItemUI.ItemSelected += OnMenuItemSelected;
			_contextMenuItemUIs.Add(menuItemUI);
		}
		
		private void RemoveLastMenuItem()
		{
			if (_contextMenuItemUIs.Count == 0)
				return;

			ContextMenuItemUI menuItemUI = _contextMenuItemUIs[^1];
			menuItemUI.ItemSelected -= OnMenuItemSelected;
			_contextMenuItemUIs.Remove(menuItemUI);
			_menuItemPool.ReleaseToPool(menuItemUI.gameObject);
		}

		private void OnMenuItemSelected() => CloseMenu();

		// ReSharper disable Unity.PerformanceAnalysis
		private void BuildContextMenuItems()
		{
			List<ContextMenuItem> menuItems = _contextMenuUser.AllMenuItems.Where(item => item.IsVisible).ToList();
			
			if (_contextMenuItemUIs.Count != menuItems.Count)
			{
				Debug.LogWarning("Unable to build context menu items. Collection lengths don't match", this);
				return;
			}

			for (int index = 0; index < menuItems.Count; index++)
			{
				ContextMenuItem menuItem = _contextMenuUser.AllMenuItems[index];
				
				if (!menuItem.IsVisible)
					continue;
				
				_contextMenuItemUIs[index].SetContextMenuItem(menuItem);
			}
		}
	}
}