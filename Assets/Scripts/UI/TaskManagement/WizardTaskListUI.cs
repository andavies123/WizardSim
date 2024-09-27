using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameObjectPools;
using UI.TaskManagement.WizardEventArgs;
using UnityEngine;
using GameWorld.Characters.Wizards.Tasks;
using TaskSystem.Interfaces;
using TMPro;
using Utilities.Attributes;

namespace UI.TaskManagement
{
	internal class WizardTaskListUI : MonoBehaviour
	{
		[SerializeField, Required] private WizardTaskUI taskUIPrefab;
		[SerializeField, Required] private Transform taskContainer;
		[SerializeField, Required] private Transform inactiveTaskContainer;
		[SerializeField, Required] private TMP_Dropdown sortByDropdown;

		private IGameObjectPool _taskUIObjectPool;
		private List<WizardTaskUI> _taskUIs = new();

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
			WizardTaskUI taskUI = _taskUIObjectPool.GetFromPool(taskContainer).GetComponent<WizardTaskUI>();
			taskUI.gameObject.name = $"Task UI #{_taskUIs.Count + 1}";
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
			_taskUIObjectPool.ReleaseToPool(taskUI.gameObject);
		}

		private void Awake()
		{
			_taskUIObjectPool = new GameObjectPool(taskUIPrefab.gameObject, inactiveTaskContainer, 10, 20);
		}

		private void Start()
		{
			sortByDropdown.onValueChanged.AddListener(OnSortByDropdownValueChanged);
		}

		private void OnDestroy()
		{
			sortByDropdown.onValueChanged.RemoveListener(OnSortByDropdownValueChanged);
		}

		private void OnTaskCompleted(ITask completedTask)
		{
			if (completedTask is not IWizardTask task)
				return;
			
			TryRemoveTask(task);
		}

		private void OnTaskDeleted(object sender, WizardTaskUIEventArgs args)
		{
			if (args?.WizardTask != null)
			{
				TaskDeleted?.Invoke(this, new WizardTaskDeletedEventArgs(args.WizardTask));
			}
		}

		private void OnSortByDropdownValueChanged(int option)
		{
			
			switch(option)
			{
				case 0: // Sort by highest priority first
					_taskUIs = _taskUIs.OrderByDescending(taskUI => taskUI.Task.Priority).ToList();
					break;
				case 1: // Sort by lowest priority first
					_taskUIs = _taskUIs.OrderBy(taskUI => taskUI.Task.Priority).ToList();
					break;
				case 2: // Sort by oldest first
					_taskUIs = _taskUIs.OrderBy(taskUI => taskUI.Task.CreationTime).ToList();
					break;
				case 3: // Sort by newest first
					_taskUIs = _taskUIs.OrderByDescending(taskUI => taskUI.Task.CreationTime).ToList();
					break;
				default:
					Debug.LogWarning("Task Management Window sort-by option hasn't been set up yet");
					break;
			}

			// Put the UI elements in the right order
			for (int index = 0; index < _taskUIs.Count; index++)
			{
				_taskUIs[index].transform.SetSiblingIndex(index);
			}
		}
	}
}