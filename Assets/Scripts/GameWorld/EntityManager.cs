using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
	public class EntityManager : MonoBehaviour
	{
		private readonly Dictionary<Guid, Entity> _entities = new();

		public IReadOnlyDictionary<Guid, Entity> Entities => _entities;

		public void Add(Entity entity) => _entities.Add(entity.Id, entity);
		public void Remove(Entity entity) => _entities.Remove(entity.Id);
	}
}