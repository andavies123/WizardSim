using Enemies.ContextMenu.ContextMenuItems;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace Enemies.ContextMenu
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyContextMenuUser : ContextMenuUser<EnemyContextMenuItem>
	{
		[SerializeField] private InteractionEvents interactionEvents;
		
		private Enemy _enemy;

		public override string MenuTitle => _enemy.DisplayName;
		public override string InfoText { get; protected set; }

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();

			MenuItems.AddRange(new EnemyContextMenuItem[]
			{
				new IdleEnemyContextMenuItem(_enemy),
				new MoveToEnemyContextMenuItem(_enemy, interactionEvents),
				new HealEnemyPercentageContextMenuItem(_enemy, 10),
				new HurtEnemyPercentageContextMenuItem(_enemy, 10),
				new HealEnemyPercentageContextMenuItem(_enemy, 100),
				new HurtEnemyPercentageContextMenuItem(_enemy, 100),
			});
		}

		private void Update()
		{
			InfoText = _enemy.StateMachine.CurrentStateDisplayStatus;
		}
	}
}