using UnityEngine;
using Utilities;

namespace GameWorld.Tiles.ContextMenu
{
	public class SpawnEnemyContextMenuItem : TileContextMenuItem
	{
		private readonly GameEventVector3 _enemySpawnRequest;

		public SpawnEnemyContextMenuItem(Tile tile, GameEventVector3 enemySpawnRequest) : base(tile)
		{
			_enemySpawnRequest = enemySpawnRequest;
		}

		public override string MenuName => "Spawn Enemy";

		protected override void OnMenuItemSelected()
		{
			Vector2 tileWorldPosition = Tile.ParentWorld.WorldPositionFromTile(Tile, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			_enemySpawnRequest.RaiseEvent(Tile, spawnPosition);
		}
	}
}