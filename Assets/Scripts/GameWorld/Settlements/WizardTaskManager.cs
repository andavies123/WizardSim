using System;
using System.Collections.Generic;
using System.Linq;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Tasks;
using TaskSystem;
using TaskSystem.Interfaces;
using UnityEngine;

namespace GameWorld.Settlements
{
	internal class WizardTaskManager : MonoBehaviour
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

			wizard.TaskHandler.RemoveTask(taskToRemove);
		}

		public void RemoveAllTasksFromWizard(Wizard wizard)
		{
			if (!wizard)
				return;
			
			// Remove Active Task
			if (wizard.TaskHandler.HasActiveTask)
			{
				wizard.TaskHandler.RemoveTask(wizard.TaskHandler.ActiveTask);
			}
			
			// Remove all Assigned Tasks
			foreach (IWizardTask backlogTask in wizard.TaskHandler.TaskBacklog)
			{
				wizard.TaskHandler.RemoveTask(backlogTask);
			}
		}
		
		public bool TryAssignTask(IWizardTask wizardTask, Wizard wizard)
		{
			if (!wizard || wizardTask == null || wizardTask.IsAssigned)
				return false;
			
			if (!IsValidWizardType(wizard, wizardTask))
				return false;
			
			wizard.TaskHandler.TryAddTaskToBacklog(wizardTask);
			
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