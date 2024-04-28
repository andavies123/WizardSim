using GameWorld.Tiles;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace Enemies.ContextMenu
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyContextMenuUser : ContextMenuUser
	{
		[SerializeField] private InteractionEvents interactionEvents;
		
		private Enemy _enemy;

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();

			MenuItems.AddRange(new ContextMenuItem[]
			{
				new("Idle", () => print("Idling not setup"), AlwaysFalse, AlwaysTrue),
				new("Move To", () => interactionEvents.RequestInteraction(_enemy, OnInteractionCallback), AlwaysFalse, AlwaysTrue),
				new("Heal 10%", () => IncreaseHealth(.1f), IsNotAtMaxHealth, AlwaysTrue),
				new("Hurt 10%", () => DecreaseHealth(.1f), IsNotAtMinHealth, AlwaysTrue),
				new("Heal 100%", () => IncreaseHealth(1), IsNotAtMaxHealth, AlwaysTrue),
				new("Hurt 100%", () => DecreaseHealth(1), IsNotAtMinHealth, AlwaysTrue)
			});
		}

		private void OnInteractionCallback(MonoBehaviour component)
		{
			print("Moving is not setup for Enemies");
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, _enemy.Transform.position.y, tilePosition.z);
			//Enemy.StateMachine.MoveTo(moveToPosition);
			interactionEvents.EndInteraction(_enemy);
		}

		private void IncreaseHealth(float percent01) => _enemy.Health.IncreaseHealth(_enemy.Health.MaxHealth * percent01);
		private void DecreaseHealth(float percent01) => _enemy.Health.DecreaseHealth(_enemy.Health.MaxHealth * percent01);

		private bool IsNotAtMaxHealth() => !_enemy.Health.IsAtMaxHealth;
		private bool IsNotAtMinHealth() => !_enemy.Health.IsAtMinHealth;
	}
}