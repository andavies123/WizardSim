using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameObjectPools;
using TaskSystem.Interfaces;
using UnityEngine;
using Wizards.Tasks;

namespace UI.TaskManagement
{
	public class TaskListUI : MonoBehaviour
	{
		[SerializeField] private TaskUI taskUIPrefab;
		[SerializeField] private Transform taskContainer;
		[SerializeField] private Transform inactiveTaskContainer;

		protected IGameObjectPool TaskUIObjectPool;
		private readonly List<TaskUI> _taskUIs = new();

		public IReadOnlyList<TaskUI> Tasks => _taskUIs;

		public void AddTask(IWizardTask task)
		{
			if (task.IsNullThenLogWarning("Unable to add task. The given task is null", this))
				return;
			
			task.Completed += OnTaskCompleted;
			task.Deleted += OnTaskDeleted;
			
			// Get task ui from object pool
			TaskUI taskUI = TaskUIObjectPool.GetFromPool(taskContainer).GetComponent<TaskUI>();
			taskUI.SetTask(task);
			_taskUIs.Add(taskUI);
		}

		public void RemoveTask(ITask task)
		{
			if (task == null)
			{
				Debug.LogWarning("Unable to remove null task from TaskListUI");
				return;
			}
            
			TaskUI taskUI = _taskUIs.FirstOrDefault(x => x.Task.Id == task.Id);
			RemoveTaskUI(taskUI);
		}

		public void RemoveTaskUI(TaskUI taskUI)
		{
			if (!taskUI)
			{
				Debug.LogWarning("Unable to remove Task from TaskListUI");
				return;
			}

			taskUI.Task.Completed -= OnTaskCompleted;
			taskUI.Task.Deleted -= OnTaskDeleted;
			_taskUIs.Remove(taskUI);
			taskUI.ClearTask();
			TaskUIObjectPool.ReleaseToPool(taskUI.gameObject);
		}

		public void ClearTasks()
		{
			for (int index = _taskUIs.Count - 1; index >= 0; index--)
			{
				RemoveTaskUI(_taskUIs[index]);
			}
		}

		private void Awake()
		{
			taskUIPrefab.ThrowIfNull(nameof(taskUIPrefab));
			taskContainer.ThrowIfNull(nameof(taskContainer));
			inactiveTaskContainer.ThrowIfNull(nameof(inactiveTaskContainer));

			TaskUIObjectPool = new GameObjectPool(taskUIPrefab.gameObject, inactiveTaskContainer, 10, 20);
		}

		private void OnTaskCompleted(object sender, EventArgs args)
		{
			if (sender is ITask task)
			{
				RemoveTask(task);
			}
		}

		private void OnTaskDeleted(object sender, EventArgs args)
		{
			if (sender is ITask task)
			{
				RemoveTask(task);
			}
		}
	}
}