using UI.ContextMenus;

namespace GameWorld.Tiles.ContextMenu
{
	public abstract class TileContextMenuItem : ContextMenuItem
	{
		protected TileContextMenuItem(Tile tile)
		{
			Tile = tile;
		}
		
		protected Tile Tile { get; }
	}
}