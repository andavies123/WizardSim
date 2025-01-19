using Extensions;
using GameObjectPools;
using UnityEngine;

namespace UI.DamageTexts
{
	public class DamageTextFactory
	{
		private readonly Transform _activeContainer;
		
		private readonly IGameObjectPool _gameObjectPool;

		public DamageTextFactory(Transform activeContainer, Transform inactiveContainer, GameObject prefab)
		{
			_activeContainer = activeContainer.ThrowIfNull(nameof(activeContainer));
			inactiveContainer.ThrowIfNull(nameof(inactiveContainer));
			prefab.ThrowIfNull(nameof(prefab));
			
			_gameObjectPool = new GameObjectPool(prefab, inactiveContainer, 10, 30);
		}
		
		public DamageText CreateDamageText(Vector3 startingPoint, Color textColor, float damageAmount)
		{
			DamageText damageText = _gameObjectPool.GetFromPool(_activeContainer).GetComponent<DamageText>();
			damageText.transform.SetPositionAndRotation(startingPoint, Quaternion.identity);
			damageText.Init(textColor, damageAmount);
			return damageText;
		}

		public void ReleaseDamageText(DamageText releasedDamageText)
		{
			if (!releasedDamageText)
				return;
			
			_gameObjectPool.ReleaseToPool(releasedDamageText.gameObject);
		}
	}
}