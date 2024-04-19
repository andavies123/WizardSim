using StateMachines;

namespace Wizards.States
{
	public abstract class WizardState : IState
	{
		protected readonly Wizard Wizard;

		protected WizardState(Wizard wizard) => Wizard = wizard;

		public abstract string DisplayName { get; }
		public abstract string DisplayStatus { get; protected set; }
		
		public abstract void Begin();
		public abstract void Update();
		public abstract void End();
	}
}