using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuItemTree
	{
		private const string ROOT_ITEM_NAME = "ROOT";
		
		public ContextMenuItem RootMenuItem { get; } = new(ROOT_ITEM_NAME);
		public bool IsEmpty => RootMenuItem.ChildMenuItems.Count == 0;

		public void AddChildMenuItem(string path, Action menuClickCallback, Func<bool> isEnabledFunc, Func<bool> isVisibleFunc)
		{
			if (path.IsNullOrWhiteSpace())
			{
				Debug.LogWarning("Unable to add a context menu item with an invalid path");
				return;
			}
			
			string[] pathItems = path.Split('|');
			if (pathItems[0] == ROOT_ITEM_NAME)
			{
				Debug.LogWarning($"Unable to create context menu item with path: {path}\nFirst element cannot be: {ROOT_ITEM_NAME}");
				return;
			}
			
			List<ContextMenuItem> children = RootMenuItem.ChildMenuItems;
			ContextMenuItem childMenuItem = null;
			
			// Todo: There should be a check to see if a leaf node is being set as a non leaf node
			foreach (string pathItem in pathItems)
			{
				childMenuItem = children.FirstOrDefault(mi => mi.Name == pathItem);

				// If a child doesn't exist then create a new one
				if (childMenuItem == null)
				{
					childMenuItem = new ContextMenuItem(pathItem);
					children.Add(childMenuItem);
				}

				children = childMenuItem.ChildMenuItems;
			}

			if (childMenuItem == null)
			{
				Debug.LogWarning($"Issue adding menu item. {nameof(childMenuItem)} is null after looping through path");
				return;
			}
			
			childMenuItem.MenuClickCallback = menuClickCallback;
			childMenuItem.IsEnabledFunc = isEnabledFunc;
			childMenuItem.IsVisibleFunc = isVisibleFunc;
		}
	}
}