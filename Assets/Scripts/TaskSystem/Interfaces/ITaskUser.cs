namespace TaskSystem.Interfaces
{
	public interface ITaskUser<in T> where T : ITask
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
		/// Assigns a task to this user
		/// </summary>
		/// <param name="task">The task that will be assigned</param>
		void AssignTask(T task);
	}
}