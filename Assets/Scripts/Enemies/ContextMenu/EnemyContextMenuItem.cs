using UI.ContextMenus;

namespace Enemies.ContextMenu
{
	public abstract class EnemyContextMenuItem : ContextMenuItem
	{
		protected EnemyContextMenuItem(Enemy enemy)
		{
			Enemy = enemy;
		}
		
		protected Enemy Enemy { get; }
	}
}