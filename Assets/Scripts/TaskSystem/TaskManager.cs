using System.Collections.Generic;
using TaskSystem.Interfaces;
using UnityEngine;

namespace TaskSystem
{
	/// <summary>
	/// Tasking system that can queue up tasks
	/// </summary>
	public class TaskManager<T> : ITaskManager<T> where T : ITask
	{
		// Sorted by priority. Highest to Lowest
		private readonly List<T> _tasks = new();

		public IReadOnlyList<T> Tasks => _tasks;
		public int TaskCount => _tasks.Count;
		
		public void AddTask(T newTask)
		{
			if (newTask == null)
			{
				Debug.LogWarning("Unable to add task. Task is null");
				return;
			}

			// If there are no tasks or the last task has a higher priority
			// than the current task, we can just add it to the end
			if (_tasks.Count == 0 || _tasks[^1].Priority >= newTask.Priority)
			{
				_tasks.Add(newTask);
			}
			else
			{
				int index = 0;
				bool taskInserted = false;

				// Loop from highest to lowest
				while (index < _tasks.Count && !taskInserted)
				{
					ITask existingTask = _tasks[index];
					
					// Insert before the first task of a lower priority
					if (existingTask.Priority < newTask.Priority)
					{
						_tasks.Insert(index, newTask);
						taskInserted = true;
					}

					index++;
				}
				
			}
		}

		public void RemoveTask(T task)
		{
			_tasks.Remove(task);
		}
	}
}