using UnityEngine;

namespace UI.HealthBars
{
	/// <summary>
	/// Interface to describe helpful properties for displaying a health bar for this object
	/// </summary>
	public interface IHealthBarUser
	{
		/// <summary>
		/// The offset of the health bar from where (0, 0, 0) would be on the object
		/// </summary>
		public Vector3 PositionOffset { get; }
	}
}