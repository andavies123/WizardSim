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

		private void Start()
		{
			_tile = GetComponent<Tile>();
			_worldBuilder = Dependencies.GetDependency<WorldBuilder>();

			AddMenuItem(new ContextMenuItem("Spawn Wizard", SpawnWizard));
			AddMenuItem(new ContextMenuItem("Spawn Enemy", SpawnEnemy));
			AddMenuItem(new ContextMenuItem("Spawn Rock", SpawnRock));
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

		private void SpawnRock()
		{
			_worldBuilder.RockWorldBuilder.TrySpawnSingle(_tile.ParentChunk, _tile.TilePosition);
		}
	}
}