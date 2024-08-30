using Game.MessengerSystem;
using UnityEngine;

namespace GameWorld.Messages
{
	public class EnemySpawnRequestMessage : Message
	{
		public Vector3 SpawnPosition { get; set; }
	}
}