using Game.MessengerSystem;
using UI.ContextMenus;
using UnityEngine;

namespace UI.Messages
{
	public class OpenContextMenuRequest : Message
	{
		public ContextMenuUser ContextMenuUser { get; set; }
		public Vector2 ScreenPosition { get; set; }
	}
}