namespace GameWorld.Settlements.Interfaces
{
	public interface ISettlement
	{
		/// <summary>
		/// The name of the settlement given by the player
		/// </summary>
		string SettlementName { get; set; }
		
		/// <summary>
		/// Object for managing all other wizard managing objects
		/// </summary>
		ISettlementWizardManager WizardManager { get; }
	}

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
