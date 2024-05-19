using System;

namespace TaskSystem.Interfaces
{
	public interface ITask
	{
		/// <summary>
		/// Invoked when the task has been completed
		/// </summary>
		event EventHandler Completed;
		
		/// <summary>
		/// Value from 1 to 10
		/// 10 = Highest priority
		/// 1 = Lowest priority
		/// </summary>
		int Priority { get; set; }
	}
}