using System;
using System.Collections.Generic;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Tasks;

namespace GameWorld.Settlements.Interfaces
{
	public interface IWizardTaskManager
	{
		/// <summary>
		/// Raised when a new task gets added to the task manager
		/// </summary>
		event Action<IWizardTask> TaskAdded;
		
		/// <summary>
		/// Raised when an existing task gets removed from the manager
		/// </summary>
		event Action<IWizardTask> TaskRemoved;
		
		/// <summary>
		/// Raised when an existing task gets assigned to a wizard
		/// </summary>
		event Action<IWizardTask> TaskAssigned;
		
		/// <summary>
		/// Collection of all tasks for this task manager
		/// </summary>
		IReadOnlyList<IWizardTask> Tasks { get; }

		/// <summary>
		/// Adds a task to the the task manager
		/// </summary>
		/// <param name="wizardTask">The task object to add to the manager</param>
		void AddTask(IWizardTask wizardTask);

		/// <summary>
		/// Removes a task from the task manager
		/// </summary>
		/// <param name="wizardTask">The task object to remove from the manager</param>
		void RemoveTask(IWizardTask wizardTask);

		/// <summary>
		/// Tries to assign a task to a wizard.
		/// If no valid tasks exist, then no tasks will be assigned 
		/// </summary>
		/// <param name="wizard">The wizard to assign a task to</param>
		void AssignTaskToWizard(Wizard wizard);

		/// <summary>
		/// Tries to assign a specific task to a specific wizard.
		/// The task won't be assigned if it is not compatible
		/// </summary>
		/// <param name="wizardTask">The task that will be assigned to the wizard</param>
		/// <param name="wizard">The wizard that will have the task assigned to</param>
		/// <returns>True if the task was assigned. False if the task was not assigned</returns>
		bool TryAssignTask(IWizardTask wizardTask, Wizard wizard);

		/// <summary>
		/// Tries to remove a specific task from a wizard.
		/// A task is needed since a wizard can have more than one task
		/// </summary>
		/// <param name="wizard">The wizard to remove the task from</param>
		/// <param name="taskToRemove">The task to be removed from the wizard</param>
		void RemoveTaskFromWizard(Wizard wizard, IWizardTask taskToRemove);

		/// <summary>
		/// Removes all tasks from a wizard
		/// </summary>
		/// <param name="wizard">The wizard that will have all tasks removed from it</param>
		void RemoveAllTasksFromWizard(Wizard wizard);
	}
}