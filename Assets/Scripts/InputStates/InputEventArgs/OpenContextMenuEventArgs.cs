using UI.ContextMenus;
using UnityEngine;

namespace InputStates.InputEventArgs
{
	public class OpenContextMenuEventArgs
	{
		public OpenContextMenuEventArgs(ContextMenuUser contextMenuUser, Vector2 screenPosition)
		{
			ContextMenuUser = contextMenuUser;
			ScreenPosition = screenPosition;
		}
		
		public ContextMenuUser ContextMenuUser { get; }
		public Vector2 ScreenPosition { get; }
	}
}