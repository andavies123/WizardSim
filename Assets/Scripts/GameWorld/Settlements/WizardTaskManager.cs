using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.Settlements.Interfaces;
using TaskSystem;
using TaskSystem.Interfaces;

namespace GameWorld.Settlements
{
	public class WizardTaskManager : IWizardTaskManager
	{
		private readonly IWizardRepo _wizardRepo;
		private readonly ITaskManager<IWizardTask> _taskManager = new TaskManager<IWizardTask>();

		public event Action<IWizardTask> TaskAdded;
		public event Action<IWizardTask> TaskRemoved;
		public event Action<IWizardTask> TaskAssigned;
		
		public WizardTaskManager(IWizardRepo wizardRepo)
		{
			_wizardRepo = wizardRepo.ThrowIfNull(nameof(wizardRepo));
		}

		public IReadOnlyList<IWizardTask> Tasks => _taskManager.Tasks;

		public void AddTask(IWizardTask wizardTask)
		{
			if (Tasks.Contains(wizardTask))
				return;
			
			_taskManager.AddTask(wizardTask);
			wizardTask.Completed += OnTaskCompleted;
			TaskAdded?.Invoke(wizardTask);
			TryAssignTask(wizardTask);
		}

		public void RemoveTask(IWizardTask wizardTask)
		{
			if (!Tasks.Contains(wizardTask))
				return;
			
			_taskManager.RemoveTask(wizardTask);
			wizardTask.Completed -= OnTaskCompleted;
			TaskRemoved?.Invoke(wizardTask);
		}

		private void TryAssignTask(IWizardTask wizardTask)
		{
			if (wizardTask.IsAssigned)
				return;
            
			foreach (Wizard wizard in _wizardRepo.AllWizards.Values)
			{
				if (wizard.IsAssignedTask || !IsValidWizardType(wizard, wizardTask))
					continue;
				
				wizard.AssignTask(wizardTask);
			}
		}

		private void OnTaskCompleted(ITask completedTask)
		{
			if (completedTask is not IWizardTask wizardTask)
				return;
			
			RemoveTask(wizardTask);
		}
		
		public static bool IsValidWizardType(Wizard wizard, IWizardTask task)
		{
			if (task.AllowedWizardTypes.Contains(TaskWizardType.Any))
				return true;
			
			return task.AllowedWizardTypes.Contains(WizardType)

			return wizard.WizardType switch
			{
				WizardType.Earth => task.AllowedWizardTypes.Contains(TaskWizardType.Earth),
				WizardType.Water => task.AllowedWizardTypes.Contains(TaskWizardType.Water),
				WizardType.Fire => task.AllowedWizardTypes.Contains(TaskWizardType.Fire),
				WizardType.Lightning => task.AllowedWizardTypes.Contains(TaskWizardType.Lightning),
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}