using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameWorld.Characters.Wizards.Tasks;
using TaskSystem.Interfaces;
using UnityEngine;

namespace GameWorld.Characters.Wizards
{
	[RequireComponent(typeof(Wizard))]
	public class WizardTaskHandler : MonoBehaviour
	{
		private Wizard _wizard;

		public IList<IWizardTask> TaskBacklog { get; } = new List<IWizardTask>();
		public IWizardTask ActiveTask { get; private set; }
		public bool HasActiveTask => ActiveTask != null;

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();
		}

		public void TryActivateNextTask()
		{
			// Todo: Should I be removing the currently active task?
			if (ActiveTask != null)
				return;

			if (!TaskBacklog.TryRemoveFirst(out IWizardTask wizardTask))
			{
				_wizard.StateMachine.Idle(); // There are no tasks
				return;
			}
			
			ActiveTask = wizardTask;
			_wizard.StateMachine.OverrideCurrentState(ActiveTask.WizardTaskState);
			ActiveTask.Completed += OnActiveTaskCompleted;
		}

		private void OnActiveTaskCompleted(ITask completedTask)
		{
			completedTask.Completed -= OnActiveTaskCompleted;
			
			if (completedTask != ActiveTask)
				return; // Shouldn't happen

			ActiveTask.RemoveWizardAssignment();
			ActiveTask = null;
			// Todo: Should I be flagging this task for destruction?
			TryActivateNextTask();
		}

		public bool TryAddTaskToBacklog(IWizardTask task, bool addToFront = false)
		{
			if (task == null)
				return false;

			if (!task.AllowAllWizardTypes && !task.AllowedWizardTypes.Contains(_wizard.WizardType))
				return false;
			
			task.AssignWizard(_wizard);
			
			// Todo: This might include stopping current task and starting this task
			if (addToFront)
				TaskBacklog.Insert(0, task);
			else
				TaskBacklog.Add(task);
			
			// Todo: I'm not sure if this is the best place to put this
			if (ActiveTask == null)
				TryActivateNextTask();

			return true;
		}

		public void RemoveTask(IWizardTask task)
		{
			if (ActiveTask == task)
			{
				// Todo: Stop task
				// Todo: Check if task is complete
				// Todo: If task is complete then destroy task
				// Todo: If task is not complete then send task back to task pool?
				TryActivateNextTask();
			}
			
			if (!TaskBacklog.Remove(task))
				return;

			task.RemoveWizardAssignment();
		}
	}
}