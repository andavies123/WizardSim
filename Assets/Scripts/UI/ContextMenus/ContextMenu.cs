using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenu : MonoBehaviour
	{
		[SerializeField] private GameObject contextMenuItemPrefab;
		[SerializeField] private Transform contextMenuItemParent;

		private readonly List<ContextMenuItemUI> _contextMenuItemUIs = new();
		private ContextMenuUser _contextMenuUser;

		public event Action MenuClosed;

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
			gameObject.SetActive(false);
			
			if (_contextMenuUser)
			{
				_contextMenuUser.CloseMenu();
				_contextMenuUser = null;
			}

			IsOpen = false;
			MenuClosed?.Invoke();
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
			if (_contextMenuItemUIs.Count < _contextMenuUser.AllMenuItems.Count)
			{
				while (_contextMenuItemUIs.Count < _contextMenuUser.AllMenuItems.Count)
				{
					AddMenuItemToEnd();
				}
			}
			else if (_contextMenuItemUIs.Count > _contextMenuUser.AllMenuItems.Count)
			{
				while (_contextMenuItemUIs.Count > _contextMenuUser.AllMenuItems.Count)
				{
					RemoveLastMenuItem();
				}
			}
		}

		private void AddMenuItemToEnd()
		{
			ContextMenuItemUI menuItemUI = Instantiate(contextMenuItemPrefab, contextMenuItemParent).GetComponent<ContextMenuItemUI>();
			menuItemUI.ItemSelected += OnMenuItemSelected;
			_contextMenuItemUIs.Add(menuItemUI);
		}
		
		private void RemoveLastMenuItem()
		{
			if (_contextMenuItemUIs.Count == 0)
				return;

			ContextMenuItemUI menuItemUI = _contextMenuItemUIs[^1];
			menuItemUI.ItemSelected -= OnMenuItemSelected;
			_contextMenuItemUIs.RemoveAt(_contextMenuItemUIs.Count - 1);
			Destroy(menuItemUI.gameObject);
		}

		private void OnMenuItemSelected() => CloseMenu();

		private void BuildContextMenuItems()
		{
			if (_contextMenuItemUIs.Count != _contextMenuUser.AllMenuItems.Count)
			{
				Debug.LogWarning("Unable to build context menu items. Collection lengths don't match", this);
				return;
			}

			for (int index = 0; index < _contextMenuUser.AllMenuItems.Count; index++)
			{
				_contextMenuItemUIs[index].SetContextMenuItem(_contextMenuUser.AllMenuItems[index]);
			}
		}
	}
}