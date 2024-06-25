using GameWorld;
using UnityEngine;

namespace GeneralBehaviours
{
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(Rigidbody))]
	public class Movement : MonoBehaviour
	{
		private Character character;
		private Transform _transform;
		private Rigidbody _rigidbody;

		private Vector3? _targetPosition;
		private float? _maxDistanceForArrival;

		public bool IsMoving { get; private set; }

		public void SetMoveToPosition(Vector3 position, float maxDistanceForArrival)
		{
			_targetPosition = position;
			_maxDistanceForArrival = maxDistanceForArrival;
			IsMoving = true;
		}

		public void CancelMoveTo()
		{
			_targetPosition = null;
			IsMoving = false;
		}

		private void Awake()
		{
			_transform = transform;
			_rigidbody = GetComponent<Rigidbody>();
			character = GetComponent<Character>();
		}

		private void FixedUpdate()
		{
			if (!_targetPosition.HasValue)
				return;

			Vector3 currentPosition = _transform.position;
			Vector3 direction = (_targetPosition.Value - currentPosition).normalized;
			Vector3 newPosition = currentPosition + direction * (character.MovementStats.Speed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(newPosition);

			if (Vector3.Distance(_transform.position, _targetPosition.Value) <= (_maxDistanceForArrival ?? 0.1f))
			{
				_targetPosition = null;
				IsMoving = false;
			}
		}
	}
}