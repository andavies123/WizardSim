using Extensions;
using PathLineRenderers;
using System.ComponentModel;
using UnityEngine;

namespace GameWorld.Characters.GeneralComponents
{
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(Rigidbody))]
	public class Movement : MonoBehaviour
	{
		private enum MovementType { NoMovement, MoveToPoint, MoveInDirection }

		private Character _character;
		private Transform _transform;
		private Rigidbody _rigidbody;

		private MovementType _movementType;

		// MovementType.MoveToPoint variables
		private Vector3? _targetPosition;
		private float? _maxDistanceForArrival;

		// MovementType.MoveInDirection variables
		private Vector3? _moveDirection;

		private PathLineRendererObjectPool _pathLineObjectPool;
		private PathLineRenderer _pathLine;

		public bool IsMoving { get; private set; }

		public void SetMoveToPosition(Vector3 position, float maxDistanceForArrival)
		{
			_movementType = MovementType.MoveToPoint;
			_targetPosition = position;
			_maxDistanceForArrival = maxDistanceForArrival;
			IsMoving = true;
			ReleasePathLine();
			_pathLine = _pathLineObjectPool.GetPathLineRenderer();
		}

		public void SetMoveDirection(Vector3 direction)
		{
			_movementType = MovementType.MoveInDirection;
			_moveDirection = direction.normalized;
		}

		public void CancelMoveTo()
		{
			_movementType = MovementType.NoMovement;
			IsMoving = false;
			ReleasePathLine();
		}

		private void Awake()
		{
			_transform = transform;
			_rigidbody = GetComponent<Rigidbody>().ThrowIfNull(nameof(_rigidbody));
			_character = GetComponent<Character>().ThrowIfNull(nameof(_character));
			_pathLineObjectPool = PathLineRendererObjectPool.Instance.ThrowIfNull(nameof(_pathLineObjectPool));
		}

		private void OnDestroy()
		{
			if (_pathLine)
				_pathLineObjectPool.ReleasePathLineRenderer(_pathLine);
		}

		private void FixedUpdate()
		{
			switch (_movementType)
			{
				case MovementType.NoMovement: break;
				case MovementType.MoveToPoint: HandleMoveToPoint(); break;
				case MovementType.MoveInDirection: HandleMoveInDirection(); break;
				default: throw new InvalidEnumArgumentException(nameof(_movementType));
			}
		}

		private void HandleMoveToPoint()
		{
			if (!_targetPosition.HasValue)
				return;

			Vector3 currentPosition = _transform.position;
			Vector3 direction = (_targetPosition.Value - currentPosition).normalized;
			Vector3 newPosition = currentPosition + direction * (_character.MovementStats.Speed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(newPosition);

			if (_pathLine)
			{
				_pathLine.UpdateLine(new[]
				{
					_targetPosition.Value,
					_transform.position
				});
			}

			if (Vector3.Distance(_transform.position, _targetPosition.Value) <= (_maxDistanceForArrival ?? 0.1f))
			{
				CancelMoveTo();
			}
		}

		private void HandleMoveInDirection()
		{
			if (!_moveDirection.HasValue)
				return;

			Vector3 currentPosition = _transform.position;
			Vector3 newPosition = currentPosition + _moveDirection.Value * (_character.MovementStats.Speed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(newPosition);

			if (_pathLine)
			{
				_pathLine.UpdateLine(new[]
				{
					_transform.position + _moveDirection.Value * 5,
					_transform.position
				});
			}
		}

		private void ReleasePathLine()
		{
			if (_pathLine)
			{
				_pathLineObjectPool.ReleasePathLineRenderer(_pathLine);
				_pathLine = null;
			}
		}
	}
}