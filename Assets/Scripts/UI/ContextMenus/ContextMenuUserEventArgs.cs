using System;

namespace UI.ContextMenus
{
	public class ContextMenuUserEventArgs : EventArgs
	{
		public ContextMenuUserEventArgs(ContextMenuUser contextMenuUser) => ContextMenuUser = contextMenuUser;
		
		public ContextMenuUser ContextMenuUser { get; }
	}
}