using System;
using TaskSystem;
using UnityEngine;
using GameWorld.Characters.Wizards.States;

namespace GameWorld.Characters.Wizards.Tasks
{
	public abstract class WizardTask : IWizardTask
	{
		private WizardTaskState _wizardTaskState;
		private Wizard _assignedWizard;
		
		public event EventHandler Completed;
		public event EventHandler<TaskUpdatedEventArgs> Updated;
		
		public abstract TaskWizardType[] AllowedWizardTypes { get; }
		public abstract string DisplayName { get; }

		public string CurrentStatus => WizardTaskState.DisplayStatus;
		public Guid Id { get; } = Guid.NewGuid();
		public float CreationTime { get; } = Time.time;
		public int Priority { get; set; }
		public bool IsAssigned => AssignedWizard;

		public Wizard AssignedWizard
		{
			get => _assignedWizard;
			set
			{
				if (_assignedWizard != value)
				{
					_assignedWizard = value;
					Updated?.Invoke(this, new TaskUpdatedEventArgs(nameof(AssignedWizard)));
				}
			}
		}

		public WizardTaskState WizardTaskState
		{
			get => _wizardTaskState;
			protected set
			{
				_wizardTaskState = value;
				_wizardTaskState.Completed += OnTaskCompleted;
			}
		}

		protected void OnTaskCompleted(object sender, EventArgs args)
		{
			Completed?.Invoke(this, EventArgs.Empty);
		}
	}
}