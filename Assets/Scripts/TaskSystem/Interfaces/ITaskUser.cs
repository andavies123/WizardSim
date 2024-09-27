using System.Collections.Generic;

namespace TaskSystem.Interfaces
{
	public interface ITaskUser<T> where T : ITask
	{
		/// <summary>
		/// True if the user has a task assigned to them
		/// False if the user does NOT have a task assigned to them
		/// </summary>
		bool IsAssignedTask { get; }
		
		/// <summary>
		/// True if the user is available to have a task assigned to them
		/// False if the user is NOT available to have a task assigned to them
		/// </summary>
		bool CanBeAssignedTask { get; }
		
		/// <summary>
		/// The task that is currently assigned to this user
		/// </summary>
		T CurrentTask { get; }
		
		/// <summary>
		/// The current list of assigned tasks for this user in order of execution
		/// </summary>
		IReadOnlyList<T> AssignedTasks { get; }
		
		/// <summary>
		/// Assigns a task to this user
		/// </summary>
		/// <param name="task">The task that will be assigned</param>
		void AssignTask(T task);

		/// <summary>
		/// Call to unassign the task and clean up any connections to the task
		/// </summary>
		void RemoveTask(T task);
	}
}