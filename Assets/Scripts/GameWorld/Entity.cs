using System;
using Stats;
using UnityEngine;

namespace GameWorld
{
	[DisallowMultipleComponent]
	public abstract class Entity : MonoBehaviour
	{
		public abstract string DisplayName { get; }
		public abstract MovementStats MovementStats { get; }
		
		public Guid Id { get; } = Guid.NewGuid();
	}
}