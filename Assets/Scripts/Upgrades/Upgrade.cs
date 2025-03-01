using System;

namespace Upgrades
{
	public sealed class Upgrade
	{
		/// <summary>
		/// The title text that would be displayed on the upgrade card
		/// </summary>
		public string Title { get; set; }
		
		/// <summary>
		/// The description text that would be displayed on the upgrade card
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Apply the current upgrade
		/// </summary>
		public Action Apply { get; set; }
	}
}