using Game.MessengerSystem;
using UI.ContextMenus;

namespace UI.Messages
{
	public class OpenContextMenuRequest : IMessage
	{
		public OpenContextMenuRequest(ContextMenuUser contextMenuUser)
		{
			ContextMenuUser = contextMenuUser;
		}
		
		public ContextMenuUser ContextMenuUser { get; }
	}
}