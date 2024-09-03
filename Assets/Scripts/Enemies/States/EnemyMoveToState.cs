using System;
using UnityEngine;

namespace Enemies.States
{
	public class EnemyMoveToState : EnemyState
	{
		private Vector3 _moveToPosition;
		private float _maxDistanceForArrival;
		
		public EnemyMoveToState(Enemy enemy) : base(enemy) { }

		public event EventHandler ArrivedAtPosition;

		public override string DisplayName => "Moving";
		public override string DisplayStatus { get; protected set; }

		public Vector3 MoveToPosition
		{
			get => _moveToPosition;
			set
			{
				if (value == _moveToPosition)
					return;

				_moveToPosition = value;
				Enemy.Movement.SetMoveToPosition(_moveToPosition, _maxDistanceForArrival);
			}
		}

		public float MaxDistanceForArrival
		{
			get => _maxDistanceForArrival;
			set
			{
				if (Mathf.Approximately(value, _maxDistanceForArrival))
					return;

				_maxDistanceForArrival = value;
				Enemy.Movement.SetMoveToPosition(_moveToPosition, _maxDistanceForArrival);
			}
		}

		public override void Begin()
		{
			Enemy.Movement.SetMoveToPosition(MoveToPosition, MaxDistanceForArrival);
		}

		public override void Update()
		{
			if (Enemy.Movement.IsMoving)
			{
				float currentDistance = Vector3.Distance(Enemy.transform.position, MoveToPosition);
				DisplayStatus = $"Moving: {currentDistance:F1} m away";
			}
			else
			{
				DisplayStatus = "Arrived";
				ArrivedAtPosition?.Invoke(this, EventArgs.Empty);
			}
		}

		public override void End() { }
	}
}