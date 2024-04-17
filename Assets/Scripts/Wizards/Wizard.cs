﻿using System;
using Extensions;
using HealthComponents;
using UI;
using UnityEngine;
using Utilities;

namespace Wizards
{
	public class Wizard : MonoBehaviour
	{
		[SerializeField] private WizardStats stats;

		private Interactable _interactable;
		
		public Guid Id { get; } = Guid.NewGuid();
		public string Name { get; set; }
		
		public Transform Transform { get; private set; }
		public WizardStateMachine StateMachine { get; private set; }
		public WizardMovement Movement { get; private set; }
		public Health Health { get; private set; }
		
		public WizardStats Stats => stats;
		
		private void Awake()
		{
			Transform = transform;
			StateMachine = GetComponent<WizardStateMachine>();
			Movement = GetComponent<WizardMovement>();
			Health = GetComponent<Health>();
			_interactable = GetComponent<Interactable>();
			
			Name = NameGenerator.GetNewName();
		}

		private void Start()
		{
			if (_interactable)
			{
				_interactable.TitleText = Name;
				_interactable.InfoText = $"Wizard - {Health.CurrentHealth:#}/{Health.MaxHealth:#} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):#}%)";
			}
		}
	}
}