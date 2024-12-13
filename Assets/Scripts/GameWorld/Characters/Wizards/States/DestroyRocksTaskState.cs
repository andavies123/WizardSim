using System.Collections.Generic;
using System.Linq;
using GameWorld.Characters.States;
using GameWorld.WorldObjects;
using GameWorld.WorldObjects.Rocks;
using StateMachines;
using UnityEngine;

namespace GameWorld.Characters.Wizards.States
{
	public class DestroyRocksTaskState : WizardTaskState
	{
		private readonly List<Rock> _rocksLeftToDestroy;
		private readonly StateMachine _stateMachine = new();
		private readonly int _initialRockCount;
		
		private readonly MoveToWorldObjectCharacterState _moveToState;
		private readonly DestroyRockState _destroyRockState;
		private readonly WaitCharacterState _waitState;

		private Rock _targetedRock;
		
		public DestroyRocksTaskState(List<Rock> rocksToDestroy)
		{
			_moveToState = new MoveToWorldObjectCharacterState(Wizard);
			_destroyRockState = new DestroyRockState(Wizard);
			_waitState = new WaitCharacterState(Wizard);
			
			_rocksLeftToDestroy = rocksToDestroy;
			_initialRockCount = rocksToDestroy.Count;
			UpdateDisplayStatus();
			AddStateTransitions();

			rocksToDestroy.ForEach(rock => rock.WorldObject.Destroyed += OnRockDestroyed);
		}

		public override string DisplayName => $"Destroying {_rocksLeftToDestroy.Count} Rocks";
		public override string DisplayStatus { get; protected set; }

		public override void Begin()
		{
			_targetedRock = GetClosestRock();
			_moveToState.Character = Wizard;
			_moveToState.Initialize(_targetedRock.WorldObject, 2f);
			_stateMachine.SetCurrentState(_moveToState);
		}

		public override void Update()
		{
			// TODO: WHEN THE TASK COMPLETES, THE STATE DOES NOT CHANGE...
			if (IsComplete)
				return;

			_stateMachine.Update();

			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocksLeftToDestroy.Count} of {_initialRockCount} | {_stateMachine.CurrentStateDisplayStatus}";
		}

		public override void End() { }

		private void AddStateTransitions()
		{
			_stateMachine.DefaultState = _waitState;

			// Move to state transitions
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_moveToState, MoveToWorldObjectCharacterState.EXIT_REASON_ARRIVED_AT_POSITION),
				new StateTransitionTo(_destroyRockState, DestroyNextRock, () => true));
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_moveToState, MoveToWorldObjectCharacterState.EXIT_REASON_OBJECT_DOES_NOT_EXIST),
				new StateTransitionTo(_destroyRockState, DestroyNextRock, () => true));
			
			// Destroy rocks state transitions
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_destroyRockState, DestroyRockState.EXIT_REASON_ROCK_DESTROYED),
				new StateTransitionTo(_moveToState, InitMoveToState, RockExists));
		}

		private bool RockExists()
		{
			return _rocksLeftToDestroy.Count > 0;
		}
		
		private void UpdateDisplayStatus()
		{
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocksLeftToDestroy.Count}/{_initialRockCount}";
		}
		
		private void DestroyNextRock()
		{
			_destroyRockState.Initialize(_targetedRock);
		}

		private void InitMoveToState()
		{
			UpdateDisplayStatus();

			_targetedRock = GetClosestRock();
			if (!_targetedRock)
			{
				CompleteTask();
				return;
			}

			_moveToState.Initialize(_targetedRock.WorldObject, 2f);
		}

		private Rock GetClosestRock()
		{
			return _rocksLeftToDestroy.OrderBy(rock => Vector3.Distance(Wizard.Position, rock.transform.position))
				.FirstOrDefault();
		}

		private void OnRockDestroyed(WorldObject worldObject)
		{
			worldObject.Destroyed -= OnRockDestroyed;

			Rock removedRock = _rocksLeftToDestroy.FirstOrDefault(rock => rock.WorldObject == worldObject);
			_rocksLeftToDestroy.Remove(removedRock);

			if (_rocksLeftToDestroy.Count == 0)
			{
				CompleteTask();
			}
		}
	}
}