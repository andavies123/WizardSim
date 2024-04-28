using Game;
using GameWorld.Builders;
using UI.ContextMenus;
using UnityEngine;
using Utilities;

namespace GameWorld.Tiles.ContextMenu
{
	[RequireComponent(typeof(Tile))]
	public class TileContextMenuUser : ContextMenuUser
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

			MenuItems.AddRange(new ContextMenuItem[]
			{
				new("Spawn Wizard", SpawnWizard),
				new("Spawn Enemy", SpawnEnemy),
				new("Spawn Rock", () => _worldBuilder.RockWorldBuilder.TrySpawnSingle(_tile.ParentChunk, _tile.TilePosition))
			});
		}

		private void SpawnWizard()
		{
			Vector2 tileWorldPosition = _tile.ParentWorld.WorldPositionFromTile(_tile, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			wizardSpawnRequest.RaiseEvent(this, spawnPosition);
		}

		private void SpawnEnemy()
		{
			Vector2 tileWorldPosition = _tile.ParentWorld.WorldPositionFromTile(_tile, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			enemySpawnRequest.RaiseEvent(this, spawnPosition);
		}
	}
}