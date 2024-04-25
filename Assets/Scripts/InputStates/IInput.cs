namespace InputStates
{
	public interface IInput
	{
		public bool ShowInteractions { get; }

		public void Enable();
		public void Disable();
	}
}