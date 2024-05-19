using System.Collections.Generic;

namespace TaskSystem.Interfaces
{
	public interface ITaskManager<T> where T : ITask
	{
		IReadOnlyList<T> Tasks { get; }
		int TaskCount { get; }

		void AddTask(T newTask);
		void RemoveTask(T task);
	}
}