using StateMachines;

namespace Enemies.States
{
	public abstract class EnemyState : IState
	{
		protected readonly Enemy Enemy;

		protected EnemyState(Enemy enemy) => Enemy = enemy;

		public abstract string DisplayName { get; }
		public abstract string DisplayStatus { get; protected set; }
		
		public abstract void Begin();
		public abstract void Update();
		public abstract void End();
	}
}