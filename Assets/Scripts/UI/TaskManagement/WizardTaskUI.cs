using System;
using Extensions;
using TaskSystem;
using TMPro;
using UI.TaskManagement.WizardEventArgs;
using UnityEngine;
using UnityEngine.UI;
using Wizards.Tasks;

namespace UI.TaskManagement
{
	internal class WizardTaskUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text nameText;
		[SerializeField] private TMP_Text assignedWizardText;
		[SerializeField] private TMP_Text statusText;
		[SerializeField] private TMP_Dropdown priorityDropdown;
		[SerializeField] private Button deleteButton;

		public IWizardTask Task { get; private set; }

		public event EventHandler<WizardTaskUIEventArgs> TaskDeleted;
        
		public void SetTask(IWizardTask task)
		{
			if (task.IsNullThenLogWarning("Unable to update task UI with a null task", this))
				return;

			Task = task;
			UpdateNameText();
			UpdateAssignedWizardText();
			UpdateStatusText();
			priorityDropdown.SetValueWithoutNotify(PriorityToDropdownIndex(task.Priority));

			Task.Updated += OnTaskUpdated;
		}

		public void ClearTask()
		{
			Task.Updated -= OnTaskUpdated;
			
			Task = null;
			nameText.SetText(string.Empty);
			assignedWizardText.SetText(string.Empty);
			statusText.SetText(string.Empty);
			priorityDropdown.SetValueWithoutNotify(0);
		}

		private void UpdateNameText()
		{
			nameText.SetText(Task.DisplayName);
		}

		private void UpdateAssignedWizardText()
		{
			assignedWizardText.SetText(Task.AssignedWizard ? $"- {Task.AssignedWizard.Name}" : string.Empty);
		}

		private void UpdateStatusText()
		{
			statusText.SetText(Task.CurrentStatus);
		}

		private void Awake()
		{
			nameText.ThrowIfNull(nameof(nameText));
			assignedWizardText.ThrowIfNull(nameof(assignedWizardText));
			statusText.ThrowIfNull(nameof(statusText));
			priorityDropdown.ThrowIfNull(nameof(priorityDropdown));
			deleteButton.ThrowIfNull(nameof(deleteButton));

			priorityDropdown.onValueChanged.AddListener(OnPriorityDropdownValueChanged);
			deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		}

		private void Update()
		{
			if (Task != null)
				UpdateStatusText();
		}

		private void OnDestroy()
		{
			priorityDropdown.onValueChanged.RemoveListener(OnPriorityDropdownValueChanged);
			deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
		}

		private void OnPriorityDropdownValueChanged(int value)
		{
			Task.Priority = DropdownIndexToPriority(value);
		}

		private void OnDeleteButtonClicked()
		{
			TaskDeleted?.Invoke(this, new WizardTaskUIEventArgs(Task));
		}

		private void OnTaskUpdated(object sender, TaskUpdatedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(WizardTask.AssignedWizard):
					UpdateAssignedWizardText();
					break;
			}
		}

		#region Static Helpers
		
		private static int PriorityToDropdownIndex(int priority)
		{
			return Mathf.Clamp(priority - 1, 0, 9);
		}

		private static int DropdownIndexToPriority(int dropdownIndex)
		{
			return dropdownIndex + 1;
		}

		#endregion
	}
}