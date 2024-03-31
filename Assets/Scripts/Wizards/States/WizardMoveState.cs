using System;
using UnityEngine;

namespace Wizards.States
{
	public class WizardMoveState : WizardState
	{
		private Vector3 _startMovePosition;
		private float _startDistance;
		
		public WizardMoveState(Wizard wizard) : base(wizard) { }

		public event Action ArrivedAtPosition;

		public override string DisplayName => "Moving";
		public override string DisplayStatus { get; protected set; }

		public Vector3 MoveToPosition { get; set; }

		public override void Begin()
		{
			_startMovePosition = Wizard.Transform.position;
			_startDistance = Vector3.Distance(_startMovePosition, MoveToPosition);
			Wizard.Movement.SetMoveToPosition(MoveToPosition);
		}

		public override void Update()
		{
			if (!Wizard.Movement.IsMoving)
			{
				DisplayStatus = "Arrived";
				ArrivedAtPosition?.Invoke();
			}
			else
			{
				float currentDistance = Vector3.Distance(Wizard.transform.position, MoveToPosition);
				DisplayStatus = $"{(_startDistance - currentDistance) / _startDistance * 100:0.0}%";
			}
		}
		
		public override void End() { }
	}
}