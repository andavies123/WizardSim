using GameWorld.WorldObjects;
using GeneralClasses.Health;

namespace GameWorld.Characters
{
	public class CharacterProperties : Properties
	{
		public InteractableProperties InteractableProperties { get; set; }
		public HealthProperties HealthProperties { get; set; }
		public DestroyedProperties DestroyedProperties { get; set; }
	}
}