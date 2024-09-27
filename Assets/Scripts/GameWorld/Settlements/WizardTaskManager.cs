using System;
using System.Collections.Generic;
using System.Linq;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.Settlements.Interfaces;
using TaskSystem;
using TaskSystem.Interfaces;

namespace GameWorld.Settlements
{
	public class WizardTaskManager : IWizardTaskManager
	{
		private readonly ITaskManager<IWizardTask> _taskManager = new TaskManager<IWizardTask>();

		public event Action<IWizardTask> TaskAdded;
		public event Action<IWizardTask> TaskRemoved;
		public event Action<IWizardTask> TaskAssigned;

		public IReadOnlyList<IWizardTask> Tasks => _taskManager.Tasks;

		public void AddTask(IWizardTask wizardTask)
		{
			if (Tasks.Contains(wizardTask))
				return;
			
			_taskManager.AddTask(wizardTask);
			wizardTask.Completed += OnTaskCompleted;
			TaskAdded?.Invoke(wizardTask);
		}

		public void RemoveTask(IWizardTask wizardTask)
		{
			if (!Tasks.Contains(wizardTask))
				return;
			
			_taskManager.RemoveTask(wizardTask);
			wizardTask.Completed -= OnTaskCompleted;
			TaskRemoved?.Invoke(wizardTask);
		}

		public void AssignTaskToWizard(Wizard wizard)
		{
			if (!wizard || Tasks.Count == 0)
				return;

			foreach (IWizardTask wizardTask in Tasks)
			{
				if (TryAssignTask(wizardTask, wizard))
					break;
			}
		}

		public void RemoveTaskFromWizard(Wizard wizard, IWizardTask taskToRemove)
		{
			if (!wizard || taskToRemove == null)
				return;

			wizard.RemoveTask(taskToRemove);
		}

		public void RemoveAllTasksFromWizard(Wizard wizard)
		{
			if (!wizard || (!wizard.IsAssignedTask && wizard.AssignedTasks.Count == 0))
				return;

			// Remove all assigned tasks
			foreach (IWizardTask wizardTask in wizard.AssignedTasks)
			{
				wizard.RemoveTask(wizardTask);
			}
			
			// Remove the current task if it exists since CurrentTask isn't held under AssignedTasks
			if (wizard.CurrentTask != null)
				wizard.RemoveTask(wizard.CurrentTask);
		}
		
		public bool TryAssignTask(IWizardTask wizardTask, Wizard wizard)
		{
			if (!wizard || wizardTask == null || wizardTask.IsAssigned)
				return false;

			if (!IsValidWizardType(wizard, wizardTask))
				return false;
			
			wizard.AssignTask(wizardTask);
			
			return wizardTask.IsAssigned;
		}

		private void OnTaskCompleted(ITask completedTask)
		{
			if (completedTask is not IWizardTask wizardTask)
				return;
			
			RemoveTask(wizardTask);
		}
		
		private static bool IsValidWizardType(Wizard wizard, IWizardTask task) =>
			task.AllowAllWizardTypes || task.AllowedWizardTypes.Contains(wizard.WizardType);
	}
}