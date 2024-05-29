using System;

namespace TaskSystem.Interfaces
{
	// Todo: Should the task have its own delete method? Should it be in charge of deleting it self?
	public interface ITask
	{
		/// <summary>
		/// Invoked when the task has been completed
		/// </summary>
		event EventHandler Completed;

		/// <summary>
		/// Invoked when the task is being deleted before completion
		/// </summary>
		event EventHandler Deleted;
		
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
		/// Call this to delete this task and raise the <see cref="Deleted"/> event
		/// </summary>
		void Delete();
	}
}