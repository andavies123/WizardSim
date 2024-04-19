namespace Enemies.ContextMenu.ContextMenuItems
{
	public class HealEnemyPercentageContextMenuItem : EnemyContextMenuItem
	{
		private readonly float _healPercentage;
		
		public HealEnemyPercentageContextMenuItem(Enemy enemy, float healPercentage) : base(enemy)
		{
			_healPercentage = healPercentage;
		}

		public override string MenuName => $"Heal {_healPercentage:#}%";

		protected override void OnMenuItemSelected()
		{
			Enemy.Health.IncreaseHealth(Enemy.Health.MaxHealth * _healPercentage / 100);
		}
	}
}