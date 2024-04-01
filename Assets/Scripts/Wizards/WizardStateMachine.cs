using GeneralBehaviours;
using UnityEngine;
using Wizards.States;

namespace Wizards
{
	[RequireComponent(typeof(Wizard))]
	public class WizardStateMachine : StateMachineComponent<WizardState>
	{
		[Header("Idle State Defaults")]
		[SerializeField] private float idleRadius = 10;
		
		private Wizard _wizard;
		private WizardIdleState _idleState;


		public void Idle()
		{
			_idleState.IdleRadius = idleRadius;
			StateMachine.SetCurrentState(_idleState);
		}

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();

			_idleState = new WizardIdleState(_wizard);
		}

		private void Start()
		{
			Idle();
		}

		private void Update()
		{
			StateMachine.Update();
		}
	}
}