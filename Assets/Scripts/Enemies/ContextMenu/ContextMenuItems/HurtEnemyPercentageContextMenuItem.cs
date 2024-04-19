namespace Enemies.ContextMenu.ContextMenuItems
{
	public class HurtEnemyPercentageContextMenuItem : EnemyContextMenuItem
	{
		private readonly float _hurtPercentage;
		
		public HurtEnemyPercentageContextMenuItem(Enemy wizard, float hurtPercentage) : base(wizard)
		{
			_hurtPercentage = hurtPercentage;
		}

		public override string MenuName => $"Hurt {_hurtPercentage:#}%";

		protected override void OnMenuItemSelected()
		{
			Enemy.Health.DecreaseHealth(Enemy.Health.MaxHealth * _hurtPercentage / 100);
		}
	}
}