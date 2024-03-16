using System;

namespace UI
{
	public abstract class ContextMenuItem
	{
		public abstract string MenuName { get; }
		public Action MenuItemSelectedAction => OnMenuItemSelected;
		
		protected abstract void OnMenuItemSelected();
		
	}
}