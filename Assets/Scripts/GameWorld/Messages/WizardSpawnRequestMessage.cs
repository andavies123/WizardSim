using Game.MessengerSystem;
using UnityEngine;
using Wizards;

namespace GameWorld.Messages
{
	public class WizardSpawnRequestMessage : IMessage
	{
		public WizardSpawnRequestMessage(Vector3 spawnPosition, WizardType wizardType)
		{
			SpawnPosition = spawnPosition;
			WizardType = wizardType;
		}
		
		public Vector3 SpawnPosition { get; }
		public WizardType WizardType { get; }
	}
}