using Enemies.States;
using GeneralBehaviours;
using UnityEngine;

namespace Enemies
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyStateMachine : StateMachineComponent<EnemyState>
	{
		[Header("Idle State Defaults")]
		[SerializeField] private float idleRadius = 10;
		
		private Enemy _enemy;
		//private EnemyIdleState _idleState;
		//private EnemyMoveToState _moveToToState;
		
		// public void Idle()
		// {
		// 	_idleState.IdleRadius = idleRadius;
		// 	StateMachine.SetCurrentState(_idleState);
		// }
		//
		// public void MoveTo(Vector3 position)
		// {
		// 	_moveToToState.MoveToPosition = position;
		// 	StateMachine.SetCurrentState(_moveToToState);
		// }

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();

			//_idleState = new WizardIdleState(_enemy);
			//_moveToToState = new WizardMoveToState(_enemy);
		}

		private void Start()
		{
			//Idle();
		}

		private void Update()
		{
			StateMachine.Update();
		}
	}
}