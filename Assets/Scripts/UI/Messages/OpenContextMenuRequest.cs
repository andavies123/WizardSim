using Game.MessengerSystem;
using UI.ContextMenus;
using UnityEngine;

namespace UI.Messages
{
	public class OpenContextMenuRequest : Message
	{
		public OpenContextMenuRequest(object sender, ContextMenuUser contextMenuUser, Vector2 screenPosition) : base(sender)
		{
			ContextMenuUser = contextMenuUser;
			ScreenPosition = screenPosition;
		}
		
		public ContextMenuUser ContextMenuUser { get; }
		public Vector2 ScreenPosition { get; }
	}
}