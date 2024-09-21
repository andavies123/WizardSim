using System;
using System.Collections.Generic;
using GameWorld.Characters.Wizards.Tasks;

namespace GameWorld.Settlements
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
	}
}