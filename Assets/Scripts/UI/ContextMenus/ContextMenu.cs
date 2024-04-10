using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ContextMenus
{
	public class ContextMenu : MonoBehaviour
	{
		[SerializeField] private GameObject contextMenuItemPrefab;
		[SerializeField] private Transform contextMenuItemParent;
		[SerializeField] private ContextMenuEvents contextMenuEvents;

		[Header("UI Components")]
		[SerializeField] private TMP_Text titleText;
		[SerializeField] private TMP_Text infoText;

		private readonly List<ContextMenuItemUI> _contextMenuItemUIs = new();
		private ContextMenuUser _contextMenuUser;

		private void Awake()
		{
			contextMenuEvents.ContextMenuOpenRequested += OnContextMenuOpenRequested;
		}

		private void OnDestroy()
		{
			contextMenuEvents.ContextMenuOpenRequested -= OnContextMenuOpenRequested;
		}

		private void Update()
		{
			if (_contextMenuUser)
				infoText.SetText(_contextMenuUser.InfoText);
			
			if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
				DisableMenu();
		}

		private void OnContextMenuOpenRequested(ContextMenuUser contextMenuUser)
		{
			if (_contextMenuUser)
				_contextMenuUser.CloseMenu();
			
			_contextMenuUser = contextMenuUser;
			EnableMenu();
		}

		private void EnableMenu()
		{
			BuildContextMenu();
			
			gameObject.SetActive(true);
		}
		
		private void DisableMenu()
		{
			gameObject.SetActive(false);
			
			if (_contextMenuUser)
			{
				_contextMenuUser.CloseMenu();
				_contextMenuUser = null;
			}
		}

		private void BuildContextMenu()
		{
			if (!_contextMenuUser || _contextMenuUser.AllMenuItems.Count == 0)
				return;

			titleText.SetText(_contextMenuUser.MenuTitle);
			infoText.SetText(_contextMenuUser.InfoText);
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

		private void OnMenuItemSelected() => DisableMenu();

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