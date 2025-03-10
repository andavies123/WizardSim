namespace Upgrades
{
	public abstract class UpgradeGroup
	{
		protected UpgradeGroup(float selectionWeight)
		{
			SelectionWeight = selectionWeight;
		}
		
		/// <summary>
		/// The weight that this upgrade group has compared to other groups
		/// when selecting an upgrade to give to the player.
		///
		/// This is not a percentage.
		///
		/// For example, if this group has a weight of 5 and another has a weight
		/// of 10, then there is a 33% chance this group would be selected
		/// </summary>
		public float SelectionWeight { get; }

		/// <summary>
		/// Gets an upgrade from this group
		/// </summary>
		/// <returns>An upgrade related to this group</returns>
		public abstract Upgrade GetUpgrade();
	}
}