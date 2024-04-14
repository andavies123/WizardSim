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
			Vector3 tilePosition = Tile.Transform.position;

			Vector3 spawnPosition = new(
				tilePosition.x + 0.5f,
				1,
				tilePosition.z + 0.5f);
			_wizardSpawnRequest.RaiseEvent(spawnPosition);
		}
	}
}