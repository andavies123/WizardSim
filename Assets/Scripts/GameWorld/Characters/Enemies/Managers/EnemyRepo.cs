using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Characters.Enemies.Managers
{
	public class EnemyRepo : MonoBehaviour
	{
		[SerializeField, Required] private Transform enemyContainer;
		
		private readonly ConcurrentDictionary<Guid, Enemy> _allEnemies = new();

		public IReadOnlyDictionary<Guid, Enemy> AllEnemies => _allEnemies;

		public bool TryAddEnemy(Enemy enemy)
		{
			if (enemy && _allEnemies.TryAdd(enemy.Id, enemy))
			{
				enemy.transform.SetParent(enemyContainer);
				return true;
			}

			return false;
		}

		public bool TryRemoveEnemy(Enemy enemy) => enemy && TryRemoveEnemy(enemy.Id);
		public bool TryRemoveEnemy(Guid enemyId)
		{
			if (_allEnemies.TryRemove(enemyId, out Enemy removedEnemy))
			{
				removedEnemy.transform.SetParent(null);
				return true;
			}

			return false;
		}
	}
}