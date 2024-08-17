namespace Game.GameStates
{
	public interface IInputState
	{
		public bool ShowInteractions { get; }

		public void Enable();
		public void Disable();
	}
}