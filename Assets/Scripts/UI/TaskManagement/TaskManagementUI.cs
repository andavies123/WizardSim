using Extensions;
using UnityEngine;
using Wizards;
using Wizards.Tasks;

namespace UI.TaskManagement
{
	public class TaskManagementUI : MonoBehaviour
	{
		[SerializeField] private TaskListUI unassignedTaskListUI;
		[SerializeField] private TaskListUI assignedTaskListUI;
		[SerializeField] private WizardTaskManager wizardTaskManager;

		public void Open()
		{
			RepopulateTaskLists();
			
			gameObject.SetActive(true);
		}

		public void Close()
		{
			gameObject.SetActive(false);
			
			unassignedTaskListUI.ClearTasks();
			assignedTaskListUI.ClearTasks();
		}

		private void RepopulateTaskLists()
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

		#region Unity Methods

		private void Awake()
		{
			unassignedTaskListUI.ThrowIfNull(nameof(unassignedTaskListUI));
			assignedTaskListUI.ThrowIfNull(nameof(assignedTaskListUI));
			wizardTaskManager.ThrowIfNull(nameof(wizardTaskManager));

			wizardTaskManager.TaskAssigned += OnTaskAssigned;
			wizardTaskManager.TaskAdded += OnTaskAdded;
			wizardTaskManager.TaskRemoved += OnTaskRemoved;
		}

		private void OnDestroy()
		{
			wizardTaskManager.TaskAssigned -= OnTaskAssigned;
			wizardTaskManager.TaskAdded -= OnTaskAdded;
			wizardTaskManager.TaskRemoved -= OnTaskRemoved;
		}

		#endregion

		#region WizardTaskManager Event Callbacks

		private void OnTaskAssigned(object sender, WizardTaskManagerEventArgs args)
		{
			RepopulateTaskLists();
		}

		private void OnTaskAdded(object sender, WizardTaskManagerEventArgs args)
		{
			RepopulateTaskLists();
		}

		private void OnTaskRemoved(object sender, WizardTaskManagerEventArgs args)
		{
			RepopulateTaskLists();
		}

		#endregion
	}
}