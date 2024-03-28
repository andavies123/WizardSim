using UnityEngine;

namespace GameWorld.Tiles.ContextMenu
{
	public class PrintTilePositionContextMenuItem : TileContextMenuItem
	{
		public PrintTilePositionContextMenuItem(Tile tile) : base(tile) { }

		public override string MenuName => "Print Tile Position";

		protected override void OnMenuItemSelected()
		{
			Debug.Log($"{Tile.TilePosition}");
		}
	}
}