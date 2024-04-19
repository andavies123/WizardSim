using GameWorld.Tiles;
using UI;
using UnityEngine;

namespace Enemies.ContextMenu.ContextMenuItems
{
	public class MoveToEnemyContextMenuItem : EnemyContextMenuItem
	{
		private readonly InteractionEvents _interactionEvents;

		public MoveToEnemyContextMenuItem(Enemy enemy, InteractionEvents interactionEvents) : base(enemy)
		{
			_interactionEvents = interactionEvents;
		}

		public override string MenuName => "Move To";

		protected override void OnMenuItemSelected()
		{
			_interactionEvents.RequestInteraction(OnInteraction);
		}

		private void OnInteraction(MonoBehaviour component)
		{
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, Enemy.Transform.position.y, tilePosition.z);
			//Enemy.StateMachine.MoveTo(moveToPosition);
		}
	}
}