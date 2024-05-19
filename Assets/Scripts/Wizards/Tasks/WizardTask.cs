using System;
using Wizards.States;

namespace Wizards.Tasks
{
	public abstract class WizardTask : IWizardTask
	{
		private WizardTaskState _wizardTaskState;
		
		public event EventHandler Completed;
		
		public abstract TaskWizardType[] AllowedWizardTypes { get; }

		public int Priority { get; set; }

		public WizardTaskState WizardTaskState
		{
			get => _wizardTaskState;
			protected set
			{
				_wizardTaskState = value;
				_wizardTaskState.Completed += OnTaskCompleted;
			}
		}

		protected void OnTaskCompleted(object sender, EventArgs args) => Completed?.Invoke(this, EventArgs.Empty);
	}
}