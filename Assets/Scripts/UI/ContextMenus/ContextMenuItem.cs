using System;

namespace UI.ContextMenus
{
	public abstract class ContextMenuItem
	{
		public abstract string MenuName { get; }
		public Action MenuItemSelectedAction => OnMenuItemSelected;
		
		protected abstract void OnMenuItemSelected();
		
	}
}