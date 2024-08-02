using Enemies.States;
using GeneralBehaviours;
using UnityEngine;

namespace Enemies
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyStateMachine : StateMachineComponent
	{
		[Header("Idle State Defaults")]
		[SerializeField] private float idleRadius = 10;
		
		private Enemy _enemy;
		private EnemyIdleState _idleState;
		
		public void Idle()
		{
			_idleState.IdleRadius = idleRadius;
			StateMachine.SetCurrentState(_idleState);
		}

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();

			_idleState = new EnemyIdleState(_enemy);
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