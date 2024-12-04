using System;
using UnityEngine;

namespace GameWorld.WorldResources
{
	[CreateAssetMenu(menuName = "Create Town Resource", fileName = "TownResource", order = 0)]
	public class TownResource : ScriptableObject
	{
		/// <summary>
		/// The name of the resource that will be used for display purposes
		/// </summary>
		[field: SerializeField] public string DisplayName { get; private set; }
		
		/// <summary>
		/// A quick description of a resource that will be used to show more details
		/// about the specific resource
		/// </summary>
		[field: SerializeField] public string Description { get; private set; }
		
		/// <summary>
		/// The image that will be displayed for this resource
		/// </summary>
		[field: SerializeField] public Sprite DisplayImage { get; private set; }

		/// <summary>
		/// Unique ID for this specific resource
		/// </summary>
		public Guid Id { get; } = Guid.NewGuid();

		public override string ToString() => $"Town Resource - Name: {DisplayName} - Id: {Id}";
	}
}