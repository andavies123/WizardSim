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

		public void Enable()
		{
			UIState.Enable();
			InputState.Enable();
			OnEnabled();
		}

		public void Disable()
		{
			UIState.Disable();
			InputState.Disable();
			OnDisabled();
		}
	}
}