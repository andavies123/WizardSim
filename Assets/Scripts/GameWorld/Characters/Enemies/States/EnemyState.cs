using System;
using StateMachines;

namespace GameWorld.Characters.Enemies.States
{
	public abstract class EnemyState : IState
	{
		protected readonly Enemy Enemy;

		protected EnemyState(Enemy enemy) => Enemy = enemy;

		public abstract event EventHandler<string> ExitRequested;

		public abstract string DisplayName { get; }
		public abstract string DisplayStatus { get; protected set; }

		public abstract void Begin();
		public abstract void Update();
		public abstract void End();
	}
}