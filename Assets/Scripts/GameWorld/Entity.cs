using System;
using UnityEngine;

namespace GameWorld
{
	public abstract class Entity : MonoBehaviour
	{
		public abstract string DisplayName { get; }
		
		public Guid Id { get; } = Guid.NewGuid();
	}
}