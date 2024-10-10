using GameObjectPools;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Attributes;

namespace UI.HealthBars
{
	public class HealthBarFactory : MonoBehaviour
	{
		[SerializeField, Required] private HealthBar healthBarPrefab;

		private readonly Queue<HealthBar> _healthBarsToRelease = new();

		private IGameObjectPool _healthBarPool = null;
		private Transform _activeHealthBarContainer = null;
		private Transform _inactiveHealthBarContainer = null;

		public HealthBar CreateHealthBar(Vector3 position)
		{
			HealthBar healthBar = _healthBarPool.GetFromPool(_activeHealthBarContainer).GetComponent<HealthBar>();
			healthBar.Initialize();
			healthBar.transform.SetPositionAndRotation(position, Quaternion.identity);
			healthBar.ReleaseRequested += OnHealthBarReleaseRequested;
			return healthBar;
		}

		public void ReleaseHealthBar(HealthBar healthBar)
		{
			// Cleanup health bar
			healthBar.ReleaseRequested -= OnHealthBarReleaseRequested;
			healthBar.CleanUp();
			healthBar.gameObject.name = "Inactive Health Bar";

			// Release to pool
			_healthBarPool.ReleaseToPool(healthBar.gameObject);
		}

		private void OnHealthBarReleaseRequested(object sender, EventArgs args)
		{
			// Due to the fact that coroutines are used for health bars, we won't have access to gameobjects on those threads.
			// We will store the referecnes here and release them on the main thread
			_healthBarsToRelease.Enqueue(sender as HealthBar);
		}

		private void Awake()
		{
			_activeHealthBarContainer = new GameObject("Active Health Bars").transform;
			_activeHealthBarContainer.parent = transform;
			
			_inactiveHealthBarContainer = new GameObject("Inactive Health Bars").transform;
			_inactiveHealthBarContainer.parent = transform;

			_healthBarPool = new GameObjectPool(healthBarPrefab.gameObject, _inactiveHealthBarContainer, 10, 20);
		}

		private void Update()
		{
			if (_healthBarsToRelease.Count == 0)
				return;

			while (_healthBarsToRelease.TryDequeue(out HealthBar healthBar))
			{
				ReleaseHealthBar(healthBar);
			}
		}
	}
}