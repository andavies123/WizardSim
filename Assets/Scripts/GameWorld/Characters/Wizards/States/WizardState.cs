using System;
using StateMachines;

namespace GameWorld.Characters.Wizards.States
{
	public abstract class WizardState : IState
	{
		protected Wizard Wizard;

		// Constructors
		protected WizardState() { }
		protected WizardState(Wizard wizard) => SetWizard(wizard);
		
		// Events
		public abstract event EventHandler<string> ExitRequested;
		
		// Display Strings
		public abstract string DisplayName { get; }
		public abstract string DisplayStatus { get; protected set; }
		
		// Initialize Methods
		public void SetWizard(Wizard wizard)
		{
			Wizard = wizard;
			if (Wizard)
				OnWizardSet();
		}

		// State Flow Methods
		public abstract void Begin();
		public abstract void Update();
		public abstract void End();

		protected virtual void OnWizardSet() { }
	}

	public class StateExitEventArgs
	{
		public static readonly StateExitEventArgs NotInitialized = new(StateForceExitReason.NotInitialized);
		
		private StateExitEventArgs(StateForceExitReason forceExitReason) => ForceExitReason = forceExitReason;
		
		public StateForceExitReason ForceExitReason { get; }
	}

	public enum StateForceExitReason
	{
		NotInitialized
	}
}