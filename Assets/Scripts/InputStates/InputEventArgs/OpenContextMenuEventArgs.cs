using UI.ContextMenus;

namespace InputStates.InputEventArgs
{
	public class OpenContextMenuEventArgs
	{
		public OpenContextMenuEventArgs(ContextMenuUser contextMenuUser)
		{
			ContextMenuUser = contextMenuUser;
		}
		
		public ContextMenuUser ContextMenuUser { get; }
	}
}