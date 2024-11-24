namespace Game.GameStates
{
	public abstract class GameState
	{
		public abstract bool AllowCameraInputs { get; }
		public abstract bool AllowInteractions { get; } // Todo: Wire this up
		
		protected abstract IInputState InputState { get; }
		protected abstract UIState UIState { get; }
		
		protected abstract void OnEnabled();
		protected abstract void OnDisabled();

		public bool IsEnabled { get; private set; }

		public void Enable()
		{
			IsEnabled = true;
			UIState.Enable();
			InputState.Enable();
			OnEnabled();
		}

		public void Disable()
		{
			IsEnabled = false;
			UIState.Disable();
			InputState.Disable();
			OnDisabled();
		}
	}
}