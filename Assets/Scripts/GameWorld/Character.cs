﻿using System;
using Extensions;
using GeneralBehaviours.HealthBehaviours;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using Stats;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace GameWorld
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(ContextMenuUser))]
	[RequireComponent(typeof(Interactable))]
	public abstract class Character : MonoBehaviour
	{
		private CharacterProperties _characterProperties;
		
		public abstract MovementStats MovementStats { get; }
		protected abstract string CharacterType { get; }
		
		public Guid Id { get; } = Guid.NewGuid();
		public Transform Transform { get; private set; }
		public HealthComponent Health { get; private set; }
		
		protected ContextMenuUser ContextMenuUser { get; private set; }
		protected Interactable Interactable { get; private set; }

		protected virtual void Awake()
		{
			LoadProperties();
			
			Transform = transform;
			ContextMenuUser = GetComponent<ContextMenuUser>();
			
			Interactable = GetComponent<Interactable>();
			Interactable.InitializeWithProperties(_characterProperties.InteractableProperties);
		}

		protected virtual void OnDestroy() { }

		protected virtual void InitializeContextMenu()
		{
			if (Health)
				ContextMenuUser.AddHealthComponentContextMenuItems(Health);
		}
		
		private void LoadProperties()
		{
			if (!PropertiesLoader.CharacterPropertiesMap.TryGetValue(CharacterType, out _characterProperties))
			{
				Debug.LogWarning($"Character - {CharacterType} - was not found. Deleting {gameObject.name}...", this);
				gameObject.Destroy();
				return;
			}
			
			if (_characterProperties.HealthProperties != null)
			{
				Health = gameObject.AddComponent<HealthComponent>();
				Health.InitializeWithProperties(_characterProperties.HealthProperties);
			}

			if (_characterProperties.InteractableProperties != null)
			{
				Interactable = gameObject.AddComponent<Interactable>();
				Interactable.InitializeWithProperties(_characterProperties.InteractableProperties);
			}

			if (_characterProperties.DestroyedProperties != null)
			{
				// Todo: Implement
			}	

			gameObject.name = _characterProperties.Id;
		}
	}
}