using GameWorld;
using UI.TaskManagement.WizardEventArgs;
using UnityEngine;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.Settlements.Interfaces;
using Utilities.Attributes;

namespace UI.TaskManagement
{
	internal class WizardTaskManagementUI : MonoBehaviour
	{
		[SerializeField, Required] private WizardTaskListUI unassignedTaskListUI;
		[SerializeField, Required] private WizardTaskListUI assignedTaskListUI;
		[SerializeField, Required] private World world;

		private IWizardTaskManager WizardTaskManager => world.Settlement.WizardManager.TaskManager;

		public void Open()
		{
			PopulateTaskLists();
			
			WizardTaskManager.TaskAssigned += OnTaskAssigned;
			WizardTaskManager.TaskAdded += OnTaskAdded;
			WizardTaskManager.TaskRemoved += OnTaskRemoved;

			unassignedTaskListUI.TaskDeleted += OnTaskDeletedThroughUI;
			assignedTaskListUI.TaskDeleted += OnTaskDeletedThroughUI;
		}

		public void Close()
		{
			WizardTaskManager.TaskAssigned -= OnTaskAssigned;
			WizardTaskManager.TaskAdded -= OnTaskAdded;
			WizardTaskManager.TaskRemoved -= OnTaskRemoved;
			
			unassignedTaskListUI.TaskDeleted -= OnTaskDeletedThroughUI;
			assignedTaskListUI.TaskDeleted -= OnTaskDeletedThroughUI;
			
			unassignedTaskListUI.ClearTasks();
			assignedTaskListUI.ClearTasks();
		}

		private void PopulateTaskLists()
		{
			unassignedTaskListUI.ClearTasks();
			assignedTaskListUI.ClearTasks();
			
			foreach (IWizardTask task in WizardTaskManager.Tasks)
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
			
			WizardTaskManager.RemoveTask(args.DeletedTask);
		}
	}
}