using UI.TaskManagement.WizardEventArgs;
using UnityEngine;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.Settlements;
using Utilities.Attributes;

namespace UI.TaskManagement
{
	internal class WizardTaskManagementUI : MonoBehaviour
	{
		[SerializeField, Required] private WizardTaskListUI unassignedTaskListUI;
		[SerializeField, Required] private WizardTaskListUI assignedTaskListUI;
		[SerializeField, Required] private WizardTaskManager wizardTaskManager;

		public void Open()
		{
			PopulateTaskLists();
			
			wizardTaskManager.TaskAssigned += OnTaskAssigned;
			wizardTaskManager.TaskAdded += OnTaskAdded;
			wizardTaskManager.TaskRemoved += OnTaskRemoved;

			unassignedTaskListUI.TaskDeleted += OnTaskDeletedThroughUI;
			assignedTaskListUI.TaskDeleted += OnTaskDeletedThroughUI;
		}

		public void Close()
		{
			wizardTaskManager.TaskAssigned -= OnTaskAssigned;
			wizardTaskManager.TaskAdded -= OnTaskAdded;
			wizardTaskManager.TaskRemoved -= OnTaskRemoved;
			
			unassignedTaskListUI.TaskDeleted -= OnTaskDeletedThroughUI;
			assignedTaskListUI.TaskDeleted -= OnTaskDeletedThroughUI;
			
			unassignedTaskListUI.ClearTasks();
			assignedTaskListUI.ClearTasks();
		}

		private void PopulateTaskLists()
		{
			unassignedTaskListUI.ClearTasks();
			assignedTaskListUI.ClearTasks();
			
			foreach (IWizardTask task in wizardTaskManager.Tasks)
			{
				if (task.IsAssigned)
					assignedTaskListUI.AddTask(task);
				else
					unassignedTaskListUI.AddTask(task);
			}
		}

		private void OnTaskAssigned(IWizardTask assignedTask)
		{
			if (unassignedTaskListUI.TryRemoveTask(assignedTask))
				assignedTaskListUI.AddTask(assignedTask);
		}

		private void OnTaskAdded(IWizardTask addedTask)
		{
			if (addedTask.IsAssigned)
				assignedTaskListUI.AddTask(addedTask);
			else
				unassignedTaskListUI.AddTask(addedTask);
		}

		private void OnTaskRemoved(IWizardTask removedTask)
		{
			if (unassignedTaskListUI.TryRemoveTask(removedTask))
				return;

			assignedTaskListUI.TryRemoveTask(removedTask);
		}

		private void OnTaskDeletedThroughUI(object sender, WizardTaskDeletedEventArgs args)
		{
			if (args.DeletedTask == null)
			{
				Debug.LogWarning("Attempting to delete null task...");
				return;
			}
			
			wizardTaskManager.RemoveTask(args.DeletedTask);
		}
	}
}