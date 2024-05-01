using Game.MessengerSystem;
using UnityEngine;

namespace GameWorld.Messages
{
	public class EnemySpawnRequestMessage : IMessage
	{
		public EnemySpawnRequestMessage(Vector3 spawnPosition)
		{
			SpawnPosition = spawnPosition;
		}
		
		public Vector3 SpawnPosition { get; }
	}
}