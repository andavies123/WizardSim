namespace Enemies.ContextMenu.ContextMenuItems
{
	public class IdleEnemyContextMenuItem : EnemyContextMenuItem
	{
		public IdleEnemyContextMenuItem(Enemy enemy) : base(enemy) { }

		public override string MenuName => "Idle";

		protected override void OnMenuItemSelected()
		{
			//Enemy.StateMachine.Idle();
		}
	}
}