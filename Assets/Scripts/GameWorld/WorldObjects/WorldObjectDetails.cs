using System;
using System.Collections.Generic;
using GameWorld.WorldResources;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[Serializable]
	[CreateAssetMenu(menuName = "World Object Details", fileName = "New Details")]
	public class WorldObjectDetails : ScriptableObject
	{
		[Tooltip("The unique name for a world object.")]
		[field: SerializeField] public string Name { get; private set; }

		[Tooltip("The GameObject prefab that will be used to instantiate the object into the world.")] 
		[field: SerializeField] public WorldObject Prefab { get; private set; }

		[Tooltip("Placement related properties for this world object")]
		[field: SerializeField] public WorldObjectPlacementProperties PlacementProperties { get; private set; }

		[Tooltip("Interactable related properties for this world object")]
		[field: SerializeField] public InteractableRelatedProperties InteractableProperties { get; private set; }
		
		[Tooltip("Health related properties for this world object")]
		[field: SerializeField] public HealthRelatedProperties HealthProperties { get; private set; }
		
		[Tooltip("What type of town resources will increase when deleted")]
		[field: SerializeField] public List<TownResource> ResourcesWhenDestroyed { get; private set; }
	}

	[Serializable]
	public class WorldObjectPlacementProperties
	{
		[Tooltip("The size that this world object would take up when placed")]
		[field: SerializeField, Min(1)] public Vector3Int Size { get; private set; }
		
		[Tooltip("The max number of this object that can be placed on the map. Use -1 if there is no max value.")]
		[field: SerializeField, Min(-1)] public int MaxObjectsAllowed { get; private set; }
	}

	[Serializable]
	public class InteractableRelatedProperties
	{
		[Tooltip("The text that will be displayed as the title in the info box")]
		[field: SerializeField] public string Title { get; private set; }

		[Tooltip("The text that will be displayed as the secondary text in the info box")]
		[field: SerializeField] public string Description { get; private set; }
	}

	[Serializable]
	public class HealthRelatedProperties
	{
		[Tooltip("The max amount of health this entity can have")]
		[field: SerializeField, Min(1)] public int MaxHealth { get; private set; }
	}
}