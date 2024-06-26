﻿using Game.MessengerSystem;
using UnityEngine;

namespace GameWorld.Messages
{
	public class WizardSpawnRequestMessage : IMessage
	{
		public WizardSpawnRequestMessage(Vector3 spawnPosition)
		{
			SpawnPosition = spawnPosition;
		}
		
		public Vector3 SpawnPosition { get; }
	}
}