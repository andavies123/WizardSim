using UnityEngine;
using UnityEngine.Pool;

namespace GameObjectPools
{
	public class GameObjectPool : IGameObjectPool
	{
		private readonly IObjectPool<GameObject> _objectPool;
		private readonly GameObject _prefab;
		private readonly Transform _inactiveItemContainer;

		public GameObjectPool(GameObject prefab, Transform inactiveItemContainer, int defaultSize, int maxSize)
		{
			_prefab = prefab;
			_inactiveItemContainer = inactiveItemContainer;
			
			_objectPool = new ObjectPool<GameObject>(
				CreateGameObject,
				OnGetFromPool,
				OnReleaseToPool,
				OnDestroyPooledObject,
				true,
				defaultSize,
				maxSize);
		}

		public GameObject GetFromPool(Transform newParent)
		{ 
			GameObject gameObject = _objectPool.Get();
            if (newParent)
	            gameObject.transform.SetParent(newParent);
            
            return gameObject;
		}
        
		public void ReleaseToPool(GameObject gameObject) => _objectPool.Release(gameObject);

		private GameObject CreateGameObject()
		{
			return Object.Instantiate(_prefab, _inactiveItemContainer);
		}

		private void OnGetFromPool(GameObject gameObject)
		{
			gameObject.SetActive(true);
		}

		private void OnReleaseToPool(GameObject gameObject)
		{
			gameObject.SetActive(false);
			gameObject.transform.SetParent(_inactiveItemContainer);
		}

		private void OnDestroyPooledObject(GameObject gameObject)
		{
			Object.Destroy(gameObject);
		}
	}
}