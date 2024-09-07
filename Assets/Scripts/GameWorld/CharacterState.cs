using System;
using StateMachines;

namespace GameWorld
{
	public abstract class CharacterState : IState
	{
		public abstract event EventHandler<string> ExitRequested;

		protected CharacterState(Character character) => Character = character;
		
		protected Character Character { get; }
		public abstract string DisplayName { get; }
		public abstract string DisplayStatus { get; protected set; }

		public abstract void Begin();
		public abstract void Update();
		public abstract void End();
	}
}