using System;

namespace UI.ContextMenus
{
	public class ContextMenuItem
	{
		public ContextMenuItem(string name, Action menuClickCallback)
		{
			Name = name;
			MenuClickCallback = menuClickCallback;
		}
		
		public string Name { get; }
		public Action MenuClickCallback { get; }
	}
}