using System;
using GeneralBehaviours.HealthBehaviours;
using GeneralBehaviours.ShaderManagers;
using GeneralClasses.Health;
using UI;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[SelectionBase]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(HealthComponent))]
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(InteractionShaderManager))]
	public sealed class WorldObject : MonoBehaviour
	{
		[SerializeField] private HealthProperties healthProperties;
        
		public WorldObjectDetails Details { get; private set; }
		public WorldObjectPositionDetails PositionDetails { get; private set; }
		public ChunkPlacementData ChunkPlacementData { get; private set; }
		public HealthComponent Health { get; private set; }
		
		public Interactable Interactable { get; private set; }
		
		public event Action<WorldObject> Destroyed;

		public void Init(WorldObjectDetails details, ChunkPlacementData chunkPlacementData)
		{
			Details = details;
			ChunkPlacementData = chunkPlacementData;

			PositionDetails = new WorldObjectPositionDetails(transform.position, details.PlacementProperties.Size);

			gameObject.name = Details.Name;
			
			Health.InitializeWithProperties(Details.HealthProperties);
			Interactable.InitializeWithProperties(Details.InteractableProperties);
		}
		
		private void Awake()
		{
			Health = GetComponent<HealthComponent>();
			Interactable = GetComponent<Interactable>();
		}

		private void OnDestroy()
		{
			Destroyed?.Invoke(this);
		}
	}
}