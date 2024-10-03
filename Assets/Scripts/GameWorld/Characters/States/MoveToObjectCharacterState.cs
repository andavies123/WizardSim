using System;
using UnityEngine;

namespace GameWorld.Characters.States
{
	public class MoveToObjectCharacterState : CharacterState
	{
		public const string EXIT_REASON_ARRIVED_AT_POSITION = nameof(EXIT_REASON_ARRIVED_AT_POSITION);
		public const string EXIT_REASON_OBJECT_DOES_NOT_EXIST = nameof(EXIT_REASON_OBJECT_DOES_NOT_EXIST);

		private GameObject _moveToObject;
		private float _maxDistanceForArrival;

		public MoveToObjectCharacterState(Character character) : base(character) { }

		public override event EventHandler<string> ExitRequested;

		public override string DisplayName => "Moving to Object";
		public override string DisplayStatus { get; protected set; }

		public void Initialize(GameObject moveToObject, float maxDistanceForArrival)
		{
			_moveToObject = moveToObject;
			_maxDistanceForArrival = maxDistanceForArrival;
		}

		public override void Begin()
		{
			Character.Movement.SetMoveToPosition(_moveToObject.transform.position, _maxDistanceForArrival);

			if (!_moveToObject)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_OBJECT_DOES_NOT_EXIST);
				return;
			}
		}

		public override void End() { }

		public override void Update()
		{
			if (!Character)
			{
				Debug.LogError("Character doesn't exist?");
				return;
			}

			if (!_moveToObject)
			{
				ExitRequested?.Invoke(this, EXIT_REASON_OBJECT_DOES_NOT_EXIST);
				return;
			}

			if (Character.Movement.IsMoving)
			{
				float currentDistance = Vector3.Distance(Character.transform.position, _moveToObject.transform.position);
				DisplayStatus = $"Moving: {currentDistance:F1} m away";
			}
			else
			{
				DisplayStatus = "Arrived";
				ExitRequested?.Invoke(this, EXIT_REASON_ARRIVED_AT_POSITION);
			}
		}
	}
}