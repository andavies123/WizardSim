using System;
using Extensions;
using Game;
using GeneralClasses.Health.Interfaces;
using UI;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Interactable))]
	public class WorldObject : MonoBehaviour, IHealthUser
	{
		[SerializeField] private Vector2Int size;

		private WorldObjectProperties _worldObjectProperties;
        
		// Systems
		public IHealth Health { get; private set; }
		
		// Components
		public Interactable Interactable { get; private set; }

		public event EventHandler Destroyed;

		public Vector2Int Size => size;
		public Vector2Int LocalChunkPosition { get; private set; }
		
		protected virtual string ItemName => string.Empty;

		public void Init(Vector2Int localChunkPosition)
		{
			LocalChunkPosition = localChunkPosition;
		}

		protected virtual void Awake()
		{
			Interactable = gameObject.GetOrAddComponent<Interactable>();
		}

		protected virtual void Start()
		{
			LoadProperties();
			Health = GlobalFactories.HealthFactory.CreateHealth(_worldObjectProperties.HealthProperties);
			Interactable.InitializeWithProperties(_worldObjectProperties.InteractableProperties);
		}

		protected virtual void OnDestroy()
		{
			Destroyed?.Invoke(this, System.EventArgs.Empty);
		}

		private void LoadProperties()
		{
			if (!WorldObjectPropertiesLoader.WorldObjectPropertiesMap.TryGetValue(ItemName, out _worldObjectProperties))
			{
				Debug.LogWarning($"World Object - {ItemName} - was not found. Deleting {gameObject.name}...", this);
				gameObject.Destroy();
				return;
			}

			gameObject.name = _worldObjectProperties.ItemName;
		}
	}
}