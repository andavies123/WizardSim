using GameObjectPools;
using UnityEngine;
using Utilities.Attributes;

namespace UI.HealthBars
{
	public class HealthBarFactory : MonoBehaviour
	{
		[SerializeField, Required] private HealthBar healthBarPrefab;

		private IGameObjectPool _healthBarPool;
		private Transform _activeHealthBarContainer;
		private Transform _inactiveHealthBarContainer;

		public HealthBar GetHealthBar(Vector3 position)
		{
			HealthBar healthBar = _healthBarPool.GetFromPool(_activeHealthBarContainer).GetComponent<HealthBar>();
			healthBar.transform.SetPositionAndRotation(position, Quaternion.identity);
			healthBar.Initialize();
			return healthBar;
		}

		public void ReleaseHealthBar(HealthBar healthBar)
		{
			GameObject healthBarGameObject = healthBar.gameObject;
			
			// Cleanup health bar
			healthBar.CleanUp();
			healthBarGameObject.name = "Inactive Health Bar";

			// Release to pool
			_healthBarPool.ReleaseToPool(healthBarGameObject);
		}

		private void Awake()
		{
			Transform thisTransform = transform;
			
			_activeHealthBarContainer = new GameObject("Active Health Bars").transform;
			_activeHealthBarContainer.parent = thisTransform;
			
			_inactiveHealthBarContainer = new GameObject("Inactive Health Bars").transform;
			_inactiveHealthBarContainer.parent = thisTransform;

			_healthBarPool = new GameObjectPool(healthBarPrefab.gameObject, _inactiveHealthBarContainer, 10, 20);
		}
	}
}