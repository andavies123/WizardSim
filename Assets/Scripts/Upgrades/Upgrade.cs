using System;
using UnityEngine;

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
		
		/// <summary>
		/// How the upgrade should be displayed (upgrade card)
		/// </summary>
		public UpgradeDisplaySettings DisplaySettings { get; set; }
	}

	public sealed class UpgradeDisplaySettings
	{
		/// <summary>
		/// The background color of upgrade card
		/// </summary>
		public Color BackgroundColor { get; set; }
		
		/// <summary>
		/// The outline color of the upgrade card
		/// </summary>
		public Color OutlineColor { get; set; }
	}
}