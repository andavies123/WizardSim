using Game.MessengerSystem;
using UnityEngine;
using Wizards;

namespace GameWorld.Messages
{
	public class WizardSpawnRequestMessage : Message
	{
		public WizardSpawnRequestMessage(object sender, Vector3 spawnPosition, WizardType wizardType)
			: base(sender)
		{
			SpawnPosition = spawnPosition;
			WizardType = wizardType;
		}
		
		public Vector3 SpawnPosition { get; }
		public WizardType WizardType { get; }
	}
}