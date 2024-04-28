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

		public override string MenuTitle => _enemy.DisplayName;
		public override string InfoText { get; protected set; }

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();

			MenuItems.AddRange(new ContextMenuItem[]
			{
				new("Idle", () => print("Idling not setup")),
				new("Move To", () => interactionEvents.RequestInteraction(_enemy, OnInteractionCallback)),
				new("Heal 10%", () => _enemy.Health.IncreaseHealth(_enemy.Health.MaxHealth * .1f)),
				new("Hurt 10%", () => _enemy.Health.DecreaseHealth(_enemy.Health.MaxHealth * .1f)),
				new("Heal 10%", () => _enemy.Health.IncreaseHealth(_enemy.Health.MaxHealth)),
				new("Hurt 10%", () => _enemy.Health.DecreaseHealth(_enemy.Health.MaxHealth))
			});
		}

		private void Update()
		{
			InfoText = _enemy.StateMachine.CurrentStateDisplayStatus;
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
	}
}