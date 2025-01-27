using System;
using System.Collections.Concurrent;
using Extensions;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuInjections : MonoBehaviour
	{
		private readonly ConcurrentDictionary<string, ContextMenuItemTree> _menuItemTreesByType = new();

		public void InjectContextMenuOption<TFor>(string path, Action<TFor> menuClickCallback, Func<bool> isEnabledFunc, Func<bool> isVisibleFunc) where TFor : IContextMenuUser
		{
			if (!_menuItemTreesByType.TryGetValue(typeof(TFor).Name, out ContextMenuItemTree menuItemTree))
			{
				menuItemTree = new ContextMenuItemTree();
				_menuItemTreesByType[typeof(TFor).Name] = menuItemTree;
			}
			
			menuItemTree.AddChildMenuItem(path, obj => menuClickCallback?.Invoke((TFor)obj), isEnabledFunc, isVisibleFunc);
		}

		public bool TryGetMenuItemTreeByType(string type, out ContextMenuItemTree menuItemTree)
		{
			if (type.IsNullOrWhiteSpace())
			{
				menuItemTree = null;
				return false;
			}
			
			return _menuItemTreesByType.TryGetValue(type, out menuItemTree);
		}
	}
}