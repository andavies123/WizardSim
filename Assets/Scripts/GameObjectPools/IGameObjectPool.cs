using UnityEngine;

namespace GameObjectPools
{
	public interface IGameObjectPool
	{
		GameObject GetFromPool(Transform newParent);
		
		void ReleaseToPool(GameObject gameObject);
	}
}