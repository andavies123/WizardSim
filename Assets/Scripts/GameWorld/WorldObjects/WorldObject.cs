using System;
using Extensions;
using Game;
using GameWorld.WorldResources;
using GeneralBehaviours.Damageables;
using GeneralBehaviours.HealthBehaviours;
using GeneralBehaviours.ShaderManagers;
using GeneralClasses.Health;
using UI;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[SelectionBase]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Damageable))]
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
		public Damageable Damageable { get; private set; }
		
		public Interactable Interactable { get; private set; }
		
		public event Action<WorldObject> Destroyed;

		public void Init(WorldObjectDetails details, ChunkPlacementData chunkPlacementData)
		{
			Details = details;
			ChunkPlacementData = chunkPlacementData;

			PositionDetails = new WorldObjectPositionDetails(
				Globals.World.WorldPositionFromTilePosition(chunkPlacementData.TilePosition, chunkPlacementData.ChunkPosition).ToVector3(VectorSub.XSubY), 
				details.PlacementProperties.Size);

			gameObject.name = Details.Name;
			
			Health.InitializeWithProperties(Details.HealthProperties);
			Interactable.InitializeWithProperties(Details.InteractableProperties);
		}
		
		private void Awake()
		{
			Damageable = GetComponent<Damageable>();
			Health = GetComponent<HealthComponent>();
			Interactable = GetComponent<Interactable>();
			
			Damageable.Destroyed += OnDamageableDestroyed;
		}

		private void OnDestroy()
		{
			Destroyed?.Invoke(this);
		}

		private void OnDamageableDestroyed(object sender, DamageableDestroyedEventArgs eventArgs)
		{
			TownResourceStockpile stockpile = Dependencies.Get<World>().Settlement.ResourceStockpile;
			Details.ResourcesWhenDestroyed.ForEach(resource =>
			{
				stockpile.AddResources(resource, 1);
			});
			Destroy(gameObject);
		}
	}
}