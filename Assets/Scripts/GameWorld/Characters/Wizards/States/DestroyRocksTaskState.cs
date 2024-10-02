using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameWorld.Characters.States;
using GameWorld.WorldObjects;
using GameWorld.WorldObjects.Rocks;
using StateMachines;

namespace GameWorld.Characters.Wizards.States
{
	public class DestroyRocksTaskState : WizardTaskState
	{
		private readonly List<Rock> _rocks;
		private readonly StateMachine _stateMachine = new();
		private readonly int _initialRockCount;
		
		private readonly MoveToObjectCharacterState _moveToState;
		private readonly DestroyRockState _destroyRockState;
		private readonly WaitCharacterState _waitState;
		
		public DestroyRocksTaskState(List<Rock> rocks)
		{
			_moveToState = new MoveToObjectCharacterState(Wizard);
			_destroyRockState = new DestroyRockState(Wizard);
			_waitState = new WaitCharacterState(Wizard);
			
			_rocks = rocks;
			_initialRockCount = rocks.Count;
			UpdateDisplayStatus();
			AddStateTransitions();

			rocks.ForEach(rock => rock.WorldObject.Destroyed += OnRockDestroyed);
		}

		public override string DisplayName => $"Destroying {_rocks.Count} Rocks";
		public override string DisplayStatus { get; protected set; }

		public override void Begin()
		{
			_moveToState.Character = Wizard;
			_moveToState.Initialize(_rocks[0].gameObject, 2f);
			_stateMachine.SetCurrentState(_moveToState);
		}

		public override void Update()
		{
			// TODO: WHEN THE TASK COMPLETES, THE STATE DOES NOT CHANGE...
			if (IsComplete)
				return;

			_stateMachine.Update();

			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count} of {_initialRockCount} | {_stateMachine.CurrentStateDisplayStatus}";
		}

		public override void End() { }

		private void AddStateTransitions()
		{
			_stateMachine.DefaultState = _waitState;

			// Move to state transitions
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_moveToState, MoveToObjectCharacterState.EXIT_REASON_ARRIVED_AT_POSITION),
				new StateTransitionTo(_destroyRockState, DestroyNextRock, () => true));
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_moveToState, MoveToObjectCharacterState.EXIT_REASON_OBJECT_DOES_NOT_EXIST),
				new StateTransitionTo(_destroyRockState, DestroyNextRock, () => true));
			
			// Destroy rocks state transitions
			_stateMachine.AddStateTransition(
				new StateTransitionFrom(_destroyRockState, DestroyRockState.EXIT_REASON_ROCK_DESTROYED),
				new StateTransitionTo(_moveToState, MoveToNextRock, RockExists));
		}

		private bool RockExists()
		{
			return _rocks.Count > 0;
		}
		
		private void UpdateDisplayStatus()
		{
			DisplayStatus = $"Rocks Destroyed: {_initialRockCount - _rocks.Count}/{_initialRockCount}";
		}
		
		private void DestroyNextRock()
		{
			_destroyRockState.Initialize(_rocks[0]);
		}

		private void MoveToNextRock()
		{
			UpdateDisplayStatus();
			
			if (_rocks.IsEmpty())
			{
				CompleteTask();
				return;
			}

			Rock rock = _rocks[0];
			_moveToState.Initialize(rock.gameObject, 2f);
		}

		private void OnRockDestroyed(object sender, EventArgs args)
		{
			if (sender is not WorldObject removedWorldObject)
				return; // Should always be a world object

			removedWorldObject.Destroyed -= OnRockDestroyed;

			Rock removedRock = _rocks.FirstOrDefault(rock => rock.WorldObject == removedWorldObject);
			_rocks.Remove(removedRock);

			if (_rocks.Count == 0)
			{
				CompleteTask();
			}
		}
	}
}