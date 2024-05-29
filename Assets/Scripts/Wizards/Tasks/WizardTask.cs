using System;
using UnityEngine;
using Wizards.States;

namespace Wizards.Tasks
{
	public abstract class WizardTask : IWizardTask
	{
		private WizardTaskState _wizardTaskState;
		
		public event EventHandler Completed;
		public event EventHandler Deleted;
		
		public abstract TaskWizardType[] AllowedWizardTypes { get; }
		public abstract string DisplayName { get; }

		public string CurrentStatus => WizardTaskState.DisplayStatus;
		public Guid Id { get; } = Guid.NewGuid();
		public float CreationTime { get; } = Time.time;
		public int Priority { get; set; }
		public Wizard AssignedWizard { get; set; }

		public WizardTaskState WizardTaskState
		{
			get => _wizardTaskState;
			protected set
			{
				_wizardTaskState = value;
				_wizardTaskState.Completed += OnTaskCompleted;
			}
		}

		public void Delete()
		{
			Deleted?.Invoke(this, EventArgs.Empty);
		}

		protected void OnTaskCompleted(object sender, EventArgs args)
		{
			Completed?.Invoke(this, EventArgs.Empty);
		}
	}
}