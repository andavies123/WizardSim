using Game.MessengerSystem;
using UI.ContextMenus;
using UnityEngine;

namespace UI.Messages
{
	public class OpenContextMenuRequest : IMessage
	{
		public OpenContextMenuRequest(ContextMenuUser contextMenuUser, Vector2 screenPosition)
		{
			ContextMenuUser = contextMenuUser;
			ScreenPosition = screenPosition;
		}
		
		public ContextMenuUser ContextMenuUser { get; }
		public Vector2 ScreenPosition { get; }
	}
}