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

		public void Initialize(Vector3 moveToPosition, float maxDistanceForArrival)
		{
			_moveToPosition = moveToPosition;
			_maxDistanceForArrival = maxDistanceForArrival;
		}

		public override void Begin()
		{
			Enemy.Movement.SetMoveToPosition(_moveToPosition, _maxDistanceForArrival);
		}

		public override void Update()
		{
			if (!Enemy.Movement.IsMoving)
			{
				DisplayStatus = "Arrived";
				ArrivedAtPosition?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				float currentDistance = Vector3.Distance(Enemy.transform.position, _moveToPosition);
				DisplayStatus = $"Moving: {currentDistance:F1} m away";
			}
		}
		
		public override void End() { }
	}
}