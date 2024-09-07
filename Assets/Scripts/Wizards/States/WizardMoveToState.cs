using System;
using UnityEngine;

namespace Wizards.States
{
	public class WizardMoveToState : WizardState
	{
		public const string EXIT_REASON_ARRIVED_AT_POSITION = nameof(EXIT_REASON_ARRIVED_AT_POSITION);
		
		private Vector3 _moveToPosition;
		private float _maxDistanceForArrival;
		
		public WizardMoveToState(Wizard wizard) => SetWizard(wizard);

		public override event EventHandler<string> ExitRequested;

		public override string DisplayName => "Moving";
		public override string DisplayStatus { get; protected set; }

		public void Initialize(Vector3 moveToPosition, float maxDistanceForArrival)
		{
			_moveToPosition = moveToPosition;
			_maxDistanceForArrival = maxDistanceForArrival;
		}

		public override void Begin()
		{
			Wizard.Movement.SetMoveToPosition(_moveToPosition, _maxDistanceForArrival);
		}

		public override void Update()
		{
			if (!Wizard.Movement.IsMoving)
			{
				DisplayStatus = "Arrived";
				ExitRequested?.Invoke(this, EXIT_REASON_ARRIVED_AT_POSITION);
			}
			else
			{
				float currentDistance = Vector3.Distance(Wizard.transform.position, _moveToPosition);
				DisplayStatus = $"Moving: {currentDistance:F1} m away";
			}
		}
		
		public override void End() { }
	}
}