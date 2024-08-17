using Extensions;
using GameWorld;
using PathLineRenderers;
using UnityEngine;

namespace GeneralBehaviours
{
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(Rigidbody))]
	public class Movement : MonoBehaviour
	{
		private Character _character;
		private Transform _transform;
		private Rigidbody _rigidbody;

		private Vector3? _targetPosition;
		private float? _maxDistanceForArrival;

		private PathLineRendererObjectPool _pathLineObjectPool;
		private PathLineRenderer _pathLine;

		public bool IsMoving { get; private set; }

		public void SetMoveToPosition(Vector3 position, float maxDistanceForArrival)
		{
			_targetPosition = position;
			_maxDistanceForArrival = maxDistanceForArrival;
			IsMoving = true;
			ReleasePathLine();
			_pathLine = _pathLineObjectPool.GetPathLineRenderer();
		}

		public void CancelMoveTo()
		{
			_targetPosition = null;
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

		private void FixedUpdate()
		{
			if (!_targetPosition.HasValue)
				return;

			Vector3 currentPosition = _transform.position;
			Vector3 direction = (_targetPosition.Value - currentPosition).normalized;
			Vector3 newPosition = currentPosition + direction * (_character.MovementStats.Speed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(newPosition);
			
			_pathLine.UpdateLine(new[]
			{
				_targetPosition.Value,
				transform.position
			});

			if (Vector3.Distance(_transform.position, _targetPosition.Value) <= (_maxDistanceForArrival ?? 0.1f))
			{
				CancelMoveTo();
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