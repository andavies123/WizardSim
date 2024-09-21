using System;
using System.Collections.Generic;
using System.Linq;
using Game.MessengerSystem;
using TaskSystem;
using TaskSystem.Interfaces;
using UnityEngine;
using GameWorld.Characters.Wizards.Messages;
using GameWorld.Characters.Wizards.Tasks;

namespace GameWorld.Characters.Wizards.TaskSystem
{
	public class WizardTaskManager : MonoBehaviour
	{
		[SerializeField] private WizardManager wizardManager;
		
		private readonly ITaskManager<IWizardTask> _taskManager = new TaskManager<IWizardTask>();
		private readonly Dictionary<Guid, IWizardTask> _assignedTasks = new();

		public event EventHandler<WizardTaskManagerEventArgs> TaskAdded;
		public event EventHandler<WizardTaskManagerEventArgs> TaskRemoved;
		public event EventHandler<WizardTaskManagerEventArgs> TaskAssigned;

		public IReadOnlyList<IWizardTask> Tasks => _taskManager.Tasks;
		public IReadOnlyDictionary<Guid, IWizardTask> AssignedTasks => _assignedTasks;
		
		public void AddTask(IWizardTask task)
		{
			_taskManager.AddTask(task);
			task.Completed += OnTaskCompleted;
			
			TaskAdded?.Invoke(this, new WizardTaskManagerEventArgs(task));

			wizardManager.Wizards.Values.ToList().ForEach(wizard => TryAssignTaskToWizard(wizard));
		}
		
		public void RemoveTask(IWizardTask task)
		{
			// Clear references
			_taskManager.RemoveTask(task);
			_assignedTasks.Remove(task.Id);
			
			// Remove event subscriptions
			task.Completed -= OnTaskCompleted;
			
			// Clean up on the wizard side
			if (task.AssignedWizard)
				task.AssignedWizard.UnassignTask(task);
			
			// Notify others
			TaskRemoved?.Invoke(this, new WizardTaskManagerEventArgs(task));
		}

		public bool TryAssignTaskToWizard(Wizard wizard)
		{
			if (_taskManager.TaskCount <= 0 || wizard.IsAssignedTask || !wizard.CanBeAssignedTask)
				return false;

			IWizardTask assignedTask = null;
			
			// Find a proper fit for this wizard
			foreach (IWizardTask task in _taskManager.Tasks)
			{
				if (Settlements.WizardTaskManager.IsValidWizardType(wizard, task))
				{
					assignedTask = task;
					break;
				}
			}

			// No tasks exist or there were no proper fits
			if (assignedTask == null)
				return false;
			
			wizard.AssignTask(assignedTask);
			_taskManager.RemoveTask(assignedTask);
			_assignedTasks.Add(assignedTask.Id, assignedTask);
			TaskAssigned?.Invoke(this, new WizardTaskManagerEventArgs(assignedTask));
			return true;
		}

		private void OnTaskCompleted(object sender, EventArgs args)
		{
			if (sender is not IWizardTask wizardTask)
				return;
			
			RemoveTask(wizardTask);
			TryAssignTaskToWizard(wizardTask.AssignedWizard);
		}

		private void Start()
		{
			GlobalMessenger.Subscribe<AddWizardTaskRequest>(AddWizardTaskRequestReceived);
			wizardManager.WizardAdded += OnWizardAdded;
		}

		private void OnDestroy()
		{
			GlobalMessenger.Unsubscribe<AddWizardTaskRequest>(AddWizardTaskRequestReceived);
			wizardManager.WizardAdded -= OnWizardAdded;
		}

		private void AddWizardTaskRequestReceived(AddWizardTaskRequest message)
		{
			AddTask(message.Task);
		}

		private void OnWizardAdded(object sender, WizardManagerEventArgs args)
		{
			TryAssignTaskToWizard(args.Wizard);
		}
	}
}