using System;
using GameWorld;
using StateMachines;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.States
{
	public class IdleEnemyState : EnemyState
	{
		private readonly StateMachine _stateMachine = new();
		
		private readonly WaitCharacterState _waitState;
		private readonly EnemyMoveToState _moveToState;

		private Vector3 _centerIdlePosition;

		public override event EventHandler<string> ExitRequested;

		public IdleEnemyState(Enemy enemy) : base(enemy)
		{
			_waitState = new WaitCharacterState(enemy);
			_moveToState = new EnemyMoveToState(enemy)
			{
				MaxDistanceForArrival = .5f
			};
			
			AddStateTransitions();
		}

		public override string DisplayName => "Idling";
		public override string DisplayStatus { get; protected set; }
		public float IdleRadius { get; set; }

		public override void Begin()
		{
			_stateMachine.SetCurrentState(_waitState);
		}

		public override void Update()
		{
			_stateMachine.Update();
			DisplayStatus = $"{_stateMachine.CurrentStateDisplayName} - {_stateMachine.CurrentStateDisplayStatus}";
		}
		
		public override void End() { }

		private void AddStateTransitions()
		{
			_stateMachine.AddStateTransition(
				new StateTransitionKey(_waitState, WaitCharacterState.EXIT_REASON_DONE_WAITING),
				new StateTransitionValue(_moveToState, OnDoneWaiting, () => true));
			
			_stateMachine.AddStateTransition(
				new StateTransitionKey(_moveToState, EnemyMoveToState.EXIT_REASON_ARRIVED),
				new StateTransitionValue(_waitState, OnDoneMoving, () => true));
		}

        private void OnDoneWaiting() => _moveToState.MoveToPosition = GetNextMoveToPosition();
        private void OnDoneMoving() => _waitState.WaitTime = GetRandomWaitTime();

		private Vector3 GetNextMoveToPosition()
		{
			Vector2 randomUnitCircle = Random.insideUnitCircle * IdleRadius;
			return new Vector3((int)(randomUnitCircle.x + _centerIdlePosition.x), 1, (int)(randomUnitCircle.y + _centerIdlePosition.z));
		}
		
		private static float GetRandomWaitTime() => Random.Range(5, 10);
	}
}