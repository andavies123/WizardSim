﻿using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.Tiles.ContextMenu
{
	[RequireComponent(typeof(Tile))]
	public class TileContextMenuUser : ContextMenuUser<TileContextMenuItem>
	{
		private Tile _tile;

		public override string MenuTitle => $"Tile {_tile.TilePosition}";
		public override string InfoText {get; protected set; } = "Info";

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