using System;

namespace UI.ContextMenus
{
	public class ContextMenuItem
	{
		private readonly Func<bool> _isEnabledFunc;
		private readonly Func<bool> _isVisibleFunc;
		
		public ContextMenuItem(
			string name, 
			Action menuClickCallback, 
            Func<bool> isEnabledFunc = null, 
            Func<bool> isVisibleFunc = null)
		{
			Name = name;
			MenuClickCallback = menuClickCallback;
			_isEnabledFunc = isEnabledFunc ?? AlwaysTrue;
			_isVisibleFunc = isVisibleFunc ?? AlwaysTrue;
		}
		
		public string Name { get; }
		public Action MenuClickCallback { get; }
		public bool IsEnabled { get; private set; }
		public bool IsVisible { get; private set; }

		public void RecalculateVisibility()
		{
			IsEnabled = _isEnabledFunc?.Invoke() ?? false;
			IsVisible = _isVisibleFunc?.Invoke() ?? false;
		}

		public static bool AlwaysTrue() => true;
		public static bool AlwaysFalse() => false;
	}
}