using System;

namespace GameWorld.Characters.Wizards.States
{
	public abstract class WizardTaskState : WizardState
	{
		public override event EventHandler<string> ExitRequested;
		public event EventHandler Completed;

		public bool IsComplete { get; private set; } = false;

		protected void CompleteTask()
		{
			IsComplete = true;
			Completed?.Invoke(this, EventArgs.Empty);
			ExitRequested?.Invoke(this, "Task Finished");
		}
	}
}