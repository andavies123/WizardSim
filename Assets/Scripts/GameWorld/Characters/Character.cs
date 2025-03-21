﻿using System;
using Extensions;
using GeneralBehaviours.HealthBehaviours;
using UI;
using UI.ContextMenus;
using UnityEngine;
using GameWorld.Characters.GeneralComponents;
using GeneralBehaviours.Damageables;

namespace GameWorld.Characters
{
	[SelectionBase]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CharacterDeath))]
	[RequireComponent(typeof(Damageable))]
	[RequireComponent(typeof(HealthComponent))]
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(Movement))]
	public abstract class Character : MonoBehaviour, IContextMenuUser
	{
		private CharacterProperties _characterProperties;

		public abstract CharacterStats CharacterStats { get; protected set; }
		protected abstract string CharacterType { get; }
		
		public Guid Id { get; } = Guid.NewGuid();
		public World ParentWorld { get; private set; }
		public Transform Transform { get; private set; }
		public HealthComponent Health { get; private set; }
		public Damageable Damageable { get; private set; }
		public CharacterDeath Death { get; private set; }
		public Movement Movement { get; private set; }
		protected Interactable Interactable { get; private set; }

		public Vector3 Position => Transform.position;
		
		public void Initialize(World parentWorld)
		{
			ParentWorld = parentWorld;
		}

		protected virtual void Awake()
		{
			Transform = transform;
			Damageable = GetComponent<Damageable>();
			Health = GetComponent<HealthComponent>();
			Death = GetComponent<CharacterDeath>();
			Movement = GetComponent<Movement>();
			Interactable = GetComponent<Interactable>();

			LoadProperties();
		}

		protected virtual void Start() { }
		protected virtual void OnDestroy() { }
		protected virtual void Update() { }
		
		private void LoadProperties()
		{
			if (!PropertiesLoader.CharacterPropertiesMap.TryGetValue(CharacterType, out _characterProperties))
			{
				Debug.LogWarning($"Character - {CharacterType} - was not found. Deleting {gameObject.name}...", this);
				gameObject.Destroy();
				return;
			}
			
			if (_characterProperties.HealthProperties != null)
				Health.InitializeWithProperties(_characterProperties.HealthProperties);
			if (_characterProperties.InteractableProperties != null)
				Interactable.InitializeWithProperties(_characterProperties.InteractableProperties);

			if (_characterProperties.DestroyedProperties != null)
			{
				// Todo: Implement
			}	

			gameObject.name = _characterProperties.Id;
		}
	}
}