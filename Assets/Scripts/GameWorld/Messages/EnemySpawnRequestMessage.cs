using Game.MessengerSystem;
using UnityEngine;

namespace GameWorld.Messages
{
	public class EnemySpawnRequestMessage : Message
	{
		public EnemySpawnRequestMessage(object sender, Vector3 spawnPosition) : base(sender)
		{
			SpawnPosition = spawnPosition;
		}
		
		public Vector3 SpawnPosition { get; }
	}
}