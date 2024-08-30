using Game.MessengerSystem;
using UnityEngine;
using Wizards;

namespace GameWorld.Messages
{
	public class WizardSpawnRequestMessage : Message
	{
		public Vector3 SpawnPosition { get; set; }
		public WizardType WizardType { get; set; }
	}
}