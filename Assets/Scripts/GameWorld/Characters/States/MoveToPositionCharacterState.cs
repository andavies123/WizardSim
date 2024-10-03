using System;
using UnityEngine;

namespace GameWorld.Characters.States
{
	public class MoveToPositionCharacterState : CharacterState
	{
		public const string EXIT_REASON_ARRIVED_AT_POSITION = nameof(EXIT_REASON_ARRIVED_AT_POSITION);
		
		private Vector3 _moveToPosition;
		private float _maxDistanceForArrival;
		
		public MoveToPositionCharacterState(Character character) : base(character) { }

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
			Character.Movement.SetMoveToPosition(_moveToPosition, _maxDistanceForArrival);
		}

		public override void Update()
		{
			if (!Character)
			{
				Debug.LogError("Character is dead?");
				return;
			}
            
			if (!Character.Movement.IsMoving)
			{
				DisplayStatus = "Arrived";
				ExitRequested?.Invoke(this, EXIT_REASON_ARRIVED_AT_POSITION);
			}
			else
			{
				float currentDistance = Vector3.Distance(Character.transform.position, _moveToPosition);
				DisplayStatus = $"Moving: {currentDistance:F1} m away";
			}
		}
		
		public override void End() { }
	}
}