namespace GameWorld.Settlements.Interfaces
{
	public interface IInit
	{
		/// <summary>
		/// Call this at the beginning of this objects lifecycle to
		/// run any code related to initializing the object post constructor
		/// </summary>
		void Init();
	}

	public interface ICleanUp
	{
		/// <summary>
		/// Call this at the end of this objects lifecycle to
		/// run any code related to cleaning up the object
		/// </summary>
		void CleanUp();
	}
}
