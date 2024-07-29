using System;
using Extensions;
using GeneralBehaviours.HealthBehaviours;
using UI;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Interactable))]
	public class WorldObject : MonoBehaviour
	{
		[SerializeField] private Vector2Int size;

		private WorldObjectProperties _worldObjectProperties;
		
		// Components
		public HealthComponent Health { get; private set; }
		public Interactable Interactable { get; private set; }

		public event EventHandler Destroyed;

		public Vector2Int Size => size;
		public Vector2Int LocalChunkPosition { get; private set; }
		
		protected virtual string ItemName => string.Empty;

		public void Init(Vector2Int localChunkPosition)
		{
			LocalChunkPosition = localChunkPosition;
		}

		protected virtual void UpdateInitialLocation() { }

		protected virtual void Awake()
		{
			Interactable = gameObject.GetOrAddComponent<Interactable>();
		}

		protected virtual void Start()
		{
			UpdateInitialLocation();
			LoadProperties();
			
			Interactable.InitializeWithProperties(_worldObjectProperties.InteractableProperties);
		}

		protected virtual void OnDestroy()
		{
			Destroyed?.Invoke(this, System.EventArgs.Empty);
		}

		private void LoadProperties()
		{
			if (!PropertiesLoader.WorldObjectPropertiesMap.TryGetValue(ItemName, out _worldObjectProperties))
			{
				Debug.LogWarning($"World Object - {ItemName} - was not found. Deleting {gameObject.name}...", this);
				gameObject.Destroy();
				return;
			}
			
			if (_worldObjectProperties.HealthProperties != null)
			{
				Health = gameObject.AddComponent<HealthComponent>();
				Health.InitializeWithProperties(_worldObjectProperties.HealthProperties);
			}

			gameObject.name = _worldObjectProperties.Id;
		}
	}
}