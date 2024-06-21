using System;
using System.Collections.Generic;
using GeneralClasses.Health;

namespace GameWorld.WorldObjects
{
	[Serializable]
	public class WorldObjectProperties
	{
		public string ItemName { get; set; }
		
		public InteractableProperties InteractableProperties { get; set; }
		public HealthProperties HealthProperties { get; set; }
		public DestroyedProperties DestroyedProperties { get; set; }
	}
	
	[Serializable]
	public class InteractableProperties
	{
		public string Title { get; set; }
		public string Description { get; set; }
	}

	[Serializable]
	public class DestroyedProperties
	{
		// On Zero Health Actions
		public const string OnZeroHealthActionDestroy = "Destroy";
		
		public int MinDrops { get; set; }
		public int MaxDrops { get; set; }
		public List<string> OnZeroHealthActions { get; set; }
	}
}