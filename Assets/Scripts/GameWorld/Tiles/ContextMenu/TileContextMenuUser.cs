using Game;
using GameWorld.Builders;
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
		private WorldBuilder _worldBuilder;

		public override string MenuTitle => $"Tile {_tile.TilePosition}";
		public override string InfoText {get; protected set; } = "Info";

		private void Start()
		{
			_tile = GetComponent<Tile>();
			_worldBuilder = Dependencies.GetDependency<WorldBuilder>();

			MenuItems.AddRange(new TileContextMenuItem[]
			{
				new SpawnWizardContextMenuItem(_tile, wizardSpawnRequest),
				new SpawnEnemyContextMenuItem(_tile, enemySpawnRequest),
				new SpawnWorldObjectContextMenuItem(_tile, "Spawn Rock", () => _worldBuilder.RockWorldBuilder.TrySpawnSingle(_tile.ParentChunk, _tile.TilePosition))
			});
		}
	}
}