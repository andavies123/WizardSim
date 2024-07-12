using System;
using System.Collections.Generic;

namespace UI.ContextMenus
{
	public class ContextMenuItem
	{
		public ContextMenuItem(string name)
		{
			Name = name;
		}
		
		public List<ContextMenuItem> ChildMenuItems { get; } = new();
		public bool IsBack { get; set; } = false;
		public string Name { get; }
		public bool IsLeaf => ChildMenuItems.Count == 0;
		
		public Action MenuClickCallback { get; set; }
		public Func<bool> IsEnabledFunc { get; set; }
		public Func<bool> IsVisibleFunc { get; set; }
        
		public bool IsEnabled { get; private set; }
		public bool IsVisible { get; private set; }

		public void RecalculateVisibility()
		{
			IsEnabled = IsEnabledFunc?.Invoke() ?? true;
			IsVisible = IsVisibleFunc?.Invoke() ?? true;
		}

		public static readonly Func<bool> AlwaysTrue = () => true;
		public static readonly Func<bool> AlwaysFalse = () => false;
	}
}