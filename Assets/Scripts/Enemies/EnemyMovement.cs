﻿using UnityEngine;

namespace Enemies
{
	[RequireComponent(typeof(Enemy))]
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyMovement : MonoBehaviour
	{
		private Transform _transform;
		private Enemy _enemy;
		private Rigidbody _rigidbody;

		private Vector3? _targetPosition;

		public bool IsMoving { get; private set; }

		public void SetMoveToPosition(Vector3 position)
		{
			_targetPosition = position;
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
			_enemy = GetComponent<Enemy>();
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			if (!_targetPosition.HasValue)
				return;

			Vector3 currentPosition = _transform.position;
			Vector3 direction = (_targetPosition.Value - currentPosition).normalized;
			Vector3 newPosition = currentPosition + direction * (_enemy.Stats.MovementSpeed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(newPosition);

			if (Vector3.Distance(_transform.position, _targetPosition.Value) <= 0.1f)
			{
				_rigidbody.MovePosition(_targetPosition.Value);
				_targetPosition = null;
				IsMoving = false;
			}
		}
	}
}