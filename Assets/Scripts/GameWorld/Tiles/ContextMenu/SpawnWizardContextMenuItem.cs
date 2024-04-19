using UnityEngine;
using Utilities;

namespace GameWorld.Tiles.ContextMenu
{
	public class SpawnWizardContextMenuItem : TileContextMenuItem
	{
		private readonly GameEventVector3 _wizardSpawnRequest;

		public SpawnWizardContextMenuItem(Tile tile, GameEventVector3 wizardSpawnRequest) : base(tile)
		{
			_wizardSpawnRequest = wizardSpawnRequest;
		}

		public override string MenuName => "Spawn Wizard";

		protected override void OnMenuItemSelected()
		{
			Vector2 tileWorldPosition = Tile.ParentWorld.WorldPositionFromTile(Tile, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			_wizardSpawnRequest.RaiseEvent(Tile, spawnPosition);
		}
	}
}