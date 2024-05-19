using System;

namespace Wizards.States
{
	public abstract class WizardTaskState : WizardState
	{
		public event EventHandler Completed;

		protected void Complete()
		{
			Completed?.Invoke(this, EventArgs.Empty);
		}
	}
}