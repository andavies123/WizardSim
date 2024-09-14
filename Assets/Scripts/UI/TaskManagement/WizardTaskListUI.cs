using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameObjectPools;
using UI.TaskManagement.WizardEventArgs;
using UnityEngine;
using GameWorld.Characters.Wizards.Tasks;

namespace UI.TaskManagement
{
	internal class WizardTaskListUI : MonoBehaviour
	{
		[SerializeField] private WizardTaskUI taskUIPrefab;
		[SerializeField] private Transform taskContainer;
		[SerializeField] private Transform inactiveTaskContainer;

		protected IGameObjectPool TaskUIObjectPool;
		private readonly List<WizardTaskUI> _taskUIs = new();

		internal event EventHandler<WizardTaskDeletedEventArgs> TaskDeleted;

		public void ClearTasks()
		{
			for (int index = _taskUIs.Count - 1; index >= 0; index--)
			{
				TryRemoveTask(_taskUIs[index]);
			}
		}

		public void AddTask(IWizardTask task)
		{
			if (task.IsNullThenLogWarning("Unable to add task. The given task is null", this))
				return;
			
			// Get task ui from object pool
			WizardTaskUI taskUI = TaskUIObjectPool.GetFromPool(taskContainer).GetComponent<WizardTaskUI>();
			taskUI.TaskDeleted += OnTaskDeleted;
			task.Completed += OnTaskCompleted;
			
			taskUI.SetTask(task);
			_taskUIs.Add(taskUI);
		}

		public bool TryRemoveTask(IWizardTask task)
		{
			if (task == null)
				return false;

			int taskUIIndex = _taskUIs.FindIndex(x => x.Task.Id == task.Id);

			if (taskUIIndex == -1) // Task not found
				return false;

			WizardTaskUI taskUI = _taskUIs[taskUIIndex];
			_taskUIs.RemoveAt(taskUIIndex);
			DisposeTaskUI(taskUI);

			return true;
		}

		public bool TryRemoveTask(WizardTaskUI taskUI)
		{
			if (!taskUI)
				return false;

			_taskUIs.Remove(taskUI);
			DisposeTaskUI(taskUI);
			
			return true;
		}

		private void DisposeTaskUI(WizardTaskUI taskUI)
		{
			if (!taskUI)
				return;

			taskUI.TaskDeleted -= OnTaskDeleted;
			taskUI.Task.Completed -= OnTaskCompleted;
			taskUI.ClearTask();
			TaskUIObjectPool.ReleaseToPool(taskUI.gameObject);
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
			if (sender is IWizardTask task)
			{
				TryRemoveTask(task);
			}
		}

		private void OnTaskDeleted(object sender, WizardTaskUIEventArgs args)
		{
			if (args?.WizardTask != null)
			{
				TaskDeleted?.Invoke(this, new WizardTaskDeletedEventArgs(args.WizardTask));
			}
		}
	}
}