using GameWorld.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
	public class EntityManager : MonoBehaviour
	{
		private readonly Dictionary<Guid, Character> _entities = new();

		public IReadOnlyDictionary<Guid, Character> Entities => _entities;

		public void Add(Character character) => _entities.Add(character.Id, character);
		public void Remove(Character character) => _entities.Remove(character.Id);
	}
}