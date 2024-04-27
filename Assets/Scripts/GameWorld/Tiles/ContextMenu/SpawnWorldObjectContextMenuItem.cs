using System;

namespace GameWorld.Tiles.ContextMenu
{
	public class SpawnWorldObjectContextMenuItem : TileContextMenuItem
	{
		private readonly Action _onMenuItemSelected;
		
		public SpawnWorldObjectContextMenuItem(Tile tile, string menuName, Action onMenuItemSelected) : base(tile)
		{
			MenuName = menuName;
			_onMenuItemSelected = onMenuItemSelected;
		}

		public override string MenuName { get; }

		protected override void OnMenuItemSelected() => _onMenuItemSelected?.Invoke();
	}
}