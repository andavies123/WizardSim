using System;
using UnityEngine;

namespace UI.ContextMenus
{
	[CreateAssetMenu(menuName = "Context Menu Events", fileName = "ContextMenuEvents", order = 0)]
	public class ContextMenuEvents : ScriptableObject
	{
		public event Action<ContextMenuUser> ContextMenuOpenRequested;

		public void RequestMenuOpen(ContextMenuUser contextMenuUser)
		{
			ContextMenuOpenRequested?.Invoke(contextMenuUser);
		}
	}
}