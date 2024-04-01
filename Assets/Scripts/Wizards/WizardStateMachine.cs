using StateMachines;
using UnityEngine;
using Wizards.States;

namespace Wizards
{
	[RequireComponent(typeof(Wizard))]
	public class WizardStateMachine : MonoBehaviour
	{
		[Header("Idle State Defaults")]
		[SerializeField] private float idleRadius = 10;
		
		private readonly StateMachine<WizardState> _stateMachine = new();
		private Wizard _wizard;
		private WizardIdleState _idleState;

		public string CurrentStateDisplayName => _stateMachine.CurrentStateDisplayName;
		public string CurrentStateDisplayStatus => _stateMachine.CurrentStateDisplayStatus;

		public void Idle()
		{
			_idleState.IdleRadius = idleRadius;
			_stateMachine.SetCurrentState(_idleState);
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
			_stateMachine.Update();
		}
	}
}