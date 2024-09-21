using System;

namespace TaskSystem.Interfaces
{
	public interface ITask
	{
		/// <summary>
		/// Raised when this task has been completed
		/// </summary>
		event Action<ITask> Completed;

		/// <summary>
		/// Raised when important properties of this task are updated
		/// </summary>
		event EventHandler<TaskUpdatedEventArgs> Updated;
		
		/// <summary>
		/// Unique Id for this single instance
		/// </summary>
		Guid Id { get; }
		
		/// <summary>
		/// Value from 1 to 10
		/// 10 = Highest priority
		/// 1 = Lowest priority
		/// </summary>
		int Priority { get; set; }
		
		/// <summary>
		/// The name of the task that will be displayed
		/// </summary>
		string DisplayName { get; }
		
		/// <summary>
		/// A quick summary of the current status of this task
		/// </summary>
		string CurrentStatus { get; }
		
		/// <summary>
		/// The time this task was created in game terms
		/// Can be used for sorting
		/// </summary>
		float CreationTime { get; }
		
		/// <summary>
		/// True if this task is currently assigned
		/// False if this task is not currently assigned
		/// </summary>
		public bool IsAssigned { get; }
	}
}