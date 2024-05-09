using Game.MessengerSystem;
using GameWorld.Tiles;
using UI.ContextMenus;
using UI.Messages;
using UnityEngine;

namespace Enemies.ContextMenu
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyContextMenuUser : ContextMenuUser
	{
		private Enemy _enemy;

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();

			AddMenuItem(new ContextMenuItem("Idle", null, isEnabledFunc: ContextMenuItem.AlwaysFalse));
			AddMenuItem(new ContextMenuItem("Move To", () => GlobalMessenger.Publish(new StartInteractionRequest(OnInteractionCallback))));
			AddMenuItem(new ContextMenuItem("Heal 10%", () => IncreaseHealth(0.1f), isEnabledFunc: IsNotAtMaxHealth));
			AddMenuItem(new ContextMenuItem("Hurt 10%", () => DecreaseHealth(0.1f), isEnabledFunc: IsNotAtMinHealth));
			AddMenuItem(new ContextMenuItem("Heal 100%", () => IncreaseHealth(1), isEnabledFunc: IsNotAtMaxHealth));
			AddMenuItem(new ContextMenuItem("Hurt 100%", () => DecreaseHealth(1), isEnabledFunc: IsNotAtMinHealth));
		}

		private void OnInteractionCallback(MonoBehaviour component)
		{
			print("Moving is not setup for Enemies");
			return;
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, _enemy.Transform.position.y, tilePosition.z);
			//Enemy.StateMachine.MoveTo(moveToPosition);
			GlobalMessenger.Publish(new EndInteractionRequest());
		}

		private void IncreaseHealth(float percent01) => _enemy.Health.IncreaseHealth(_enemy.Health.MaxHealth * percent01);
		private void DecreaseHealth(float percent01) => _enemy.Health.DecreaseHealth(_enemy.Health.MaxHealth * percent01);

		private bool IsNotAtMaxHealth() => !_enemy.Health.IsAtMaxHealth;
		private bool IsNotAtMinHealth() => !_enemy.Health.IsAtMinHealth;
	}
}