using GameWorld;

namespace GameWorld.Settlements
{
	public interface ISettlement
	{
		/// <summary>
		/// The <see cref="World"/> object that this settlement is a part of
		/// </summary>
		World ParentWorld { get; }

		/// <summary>
		/// An object that holds all wizard references for this settlement
		/// </summary>
		IWizardRepo WizardRepo { get; }
	}
}
