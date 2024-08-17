using System;
using Extensions;

namespace Game.GameStates.TaskManagementGameStates
{
	public class TaskManagementGameState : GameState
	{
		private readonly TaskManagementInputState _taskManagementInputState;
		private readonly TaskManagementUIState _taskManagementUIState;

		public event EventHandler CloseMenu
		{
			add => _taskManagementInputState.CloseWindowRequested += value;
			remove => _taskManagementInputState.CloseWindowRequested -= value;
		}

		public TaskManagementGameState(TaskManagementUIState taskManagementUIState)
		{
			_taskManagementUIState = taskManagementUIState.ThrowIfNull(nameof(taskManagementUIState));
			_taskManagementInputState = new TaskManagementInputState();
		}
		
		public override bool AllowCameraInputs => false;
		public override bool AllowInteractions => false;

		protected override IInputState InputState => _taskManagementInputState;
		protected override UIState UIState => _taskManagementUIState;
		
		protected override void OnEnabled() { }
		protected override void OnDisabled() { }
	}
}