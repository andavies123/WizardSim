using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wizards.Tasks;

namespace UI.TaskManagement
{
	public class TaskUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text nameText;
		[SerializeField] private TMP_Text assignedWizardText;
		[SerializeField] private TMP_Text statusText;
		[SerializeField] private TMP_Dropdown priorityDropdown;
		[SerializeField] private Button deleteButton;

		public IWizardTask Task { get; private set; }
        
		public void SetTask(IWizardTask task)
		{
			if (task.IsNullThenLogWarning("Unable to update task UI with a null task", this))
				return;

			Task = task;
			UpdateNameText();
			UpdateAssignedWizardText();
			UpdateStatusText();
			priorityDropdown.SetValueWithoutNotify(PriorityToDropdownIndex(task.Priority));
		}

		public void ClearTask()
		{
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

		#region Unity Methods

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

		#endregion

		#region UI Event Callbacks

		private void OnPriorityDropdownValueChanged(int value)
		{
			Task.Priority = DropdownIndexToPriority(value);
		}

		private void OnDeleteButtonClicked()
		{
			Task?.Delete();
		}

		#endregion

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