using GameWorld;

namespace UI
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