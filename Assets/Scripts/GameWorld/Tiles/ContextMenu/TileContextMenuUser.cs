using UI.ContextMenus;
using UnityEngine;
using Utilities;

namespace GameWorld.Tiles.ContextMenu
{
	[RequireComponent(typeof(Tile))]
	public class TileContextMenuUser : ContextMenuUser<TileContextMenuItem>
	{
		[SerializeField] private GameEventVector3 wizardSpawnRequest;
		[SerializeField] private GameEventVector3 enemySpawnRequest;
		
		private Tile _tile;

		public override string MenuTitle => $"Tile {_tile.TilePosition}";
		public override string InfoText {get; protected set; } = "Info";

		private void Awake()
		{
			_tile = GetComponent<Tile>();

			MenuItems.AddRange(new TileContextMenuItem[]
			{
				new SpawnWizardContextMenuItem(_tile, wizardSpawnRequest),
				new SpawnEnemyContextMenuItem(_tile, enemySpawnRequest)
			});
		}
	}
}