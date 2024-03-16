using System;
using UnityEngine;

namespace UI
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