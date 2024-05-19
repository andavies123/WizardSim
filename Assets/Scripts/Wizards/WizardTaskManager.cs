using System;
using System.Linq;
using Game.MessengerSystem;
using TaskSystem;
using TaskSystem.Interfaces;
using UnityEngine;
using Wizards.Messages;
using Wizards.Tasks;

namespace Wizards
{
	public class WizardTaskManager : MonoBehaviour
	{
		[SerializeField] private WizardManager wizardManager;
        
		private readonly ITaskManager<IWizardTask> _taskManager = new TaskManager<IWizardTask>();

		public void AddTask(IWizardTask task)
		{
			_taskManager.AddTask(task);

			wizardManager.Wizards.Values.ToList().ForEach(wizard => TryAssignTaskToWizard(wizard));
		}

		public bool TryAssignTaskToWizard(Wizard wizard)
		{
			if (_taskManager.TaskCount <= 0 || wizard.IsAssignedTask || !wizard.CanBeAssignedTask)
				return false;

			IWizardTask assignedTask = null;
			
			// Find a proper fit for this wizard
			foreach (IWizardTask task in _taskManager.Tasks)
			{
				if (IsValidWizardType(wizard, task))
				{
					assignedTask = task;
					wizard.AssignTask(task);
					break;
				}
			}

			// No tasks exist or there were no proper fits
			if (assignedTask == null)
				return false;

			print($"Assigning {assignedTask.GetType()} to {wizard.Name}");
			_taskManager.RemoveTask(assignedTask);
			return true;
		}

		private void Start()
		{
			GlobalMessenger.Subscribe<AddWizardTaskRequest>(AddWizardTaskRequestReceived);
		}

		private void AddWizardTaskRequestReceived(AddWizardTaskRequest message)
		{
			AddTask(message.Task);
		}

		private static bool IsValidWizardType(Wizard wizard, IWizardTask task)
		{
			if (task.AllowedWizardTypes.Contains(TaskWizardType.Any))
				return true;

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