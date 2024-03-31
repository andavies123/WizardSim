using System;
using UnityEngine;
using Utilities;

namespace Wizards
{
	public class Wizard : MonoBehaviour
	{
		[SerializeField] private WizardStats stats;
		
		public Guid Id { get; } = Guid.NewGuid();
		public string Name { get; set; }
		
		public Transform Transform { get; private set; }
		public WizardStateMachine StateMachine { get; private set; }
		public WizardMovement Movement { get; private set; }
		public WizardStats Stats => stats;
		
		private void Awake()
		{
			Transform = transform;
			StateMachine = GetComponent<WizardStateMachine>();
			Movement = GetComponent<WizardMovement>();
			
			Name = NameGenerator.GetNewName();
		}
	}
}