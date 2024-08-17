using Extensions;
using UI.TaskManagement.WizardEventArgs;
using UnityEngine;
using Wizards.Tasks;
using Wizards.TaskSystem;

namespace UI.TaskManagement
{
	internal class WizardTaskManagementUI : MonoBehaviour
	{
		[SerializeField] private WizardTaskListUI unassignedTaskListUI;
		[SerializeField] private WizardTaskListUI assignedTaskListUI;
		[SerializeField] private WizardTaskManager wizardTaskManager;

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
			foreach (IWizardTask task in wizardTaskManager.Tasks)
			{
				unassignedTaskListUI.AddTask(task);
			}

			assignedTaskListUI.ClearTasks();
			foreach (IWizardTask task in wizardTaskManager.AssignedTasks.Values)
			{
				assignedTaskListUI.AddTask(task);
			}
		}

		private void Awake()
		{
			unassignedTaskListUI.ThrowIfNull(nameof(unassignedTaskListUI));
			assignedTaskListUI.ThrowIfNull(nameof(assignedTaskListUI));
			wizardTaskManager.ThrowIfNull(nameof(wizardTaskManager));
		}

		private void OnTaskAssigned(object sender, WizardTaskManagerEventArgs args)
		{
			if (unassignedTaskListUI.TryRemoveTask(args.WizardTask))
				assignedTaskListUI.AddTask(args.WizardTask);
		}

		private void OnTaskAdded(object sender, WizardTaskManagerEventArgs args)
		{
			if (args.WizardTask.IsAssigned)
				assignedTaskListUI.AddTask(args.WizardTask);
			else
				unassignedTaskListUI.AddTask(args.WizardTask);
		}

		private void OnTaskRemoved(object sender, WizardTaskManagerEventArgs args)
		{
			if (unassignedTaskListUI.TryRemoveTask(args.WizardTask))
				return;

			assignedTaskListUI.TryRemoveTask(args.WizardTask);
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