using System;
using Extensions;

namespace Game.GameStates.TownManagementStates
{
	public class TownManagementGameState : GameState
	{
		private readonly TownManagementInputState _townManagementInputState;
		private readonly TownManagementUIState _townManagementUIState;

		public event EventHandler CloseMenu
		{
			add => _townManagementInputState.CloseWindowRequested += value;
			remove => _townManagementInputState.CloseWindowRequested -= value;
		}

		public TownManagementGameState(TownManagementUIState taskManagementUIState)
		{
			_townManagementUIState = taskManagementUIState.ThrowIfNull(nameof(taskManagementUIState));
			_townManagementInputState = new TownManagementInputState();
		}
		
		public override bool AllowCameraInputs => false;
		public override bool AllowInteractions => false;

		protected override IInputState InputState => _townManagementInputState;
		protected override UIState UIState => _townManagementUIState;
		
		protected override void OnEnabled() { }
		protected override void OnDisabled() { }
	}
}