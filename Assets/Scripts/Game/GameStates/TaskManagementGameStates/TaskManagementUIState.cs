using Extensions;
using UI.TaskManagement;
using UnityEngine;

namespace Game.GameStates.TaskManagementGameStates
{
	public class TaskManagementUIState : UIState
	{
		[SerializeField] private WizardTaskManagementUI taskManagementUI;

		protected override void OnStateEnabled() => taskManagementUI.Open();
		protected override void OnStateDisabled() => taskManagementUI.Close();

		protected override void Awake()
		{
			base.Awake();
			taskManagementUI.ThrowIfNull(nameof(taskManagementUI));
		}
	}
}