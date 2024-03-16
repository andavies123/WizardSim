using GameWorld;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(Tile))]
	public class TileContextMenuUser : ContextMenuUser<TileContextMenuItem>
	{
		private Tile _tile;

		public override string MenuTitle => $"Tile {_tile.TilePosition}";

		private void Awake()
		{
			_tile = GetComponent<Tile>();

			MenuItems.AddRange(new[]
			{
				new PrintTilePositionContextMenuItem(_tile)
			});
		}
	}
}