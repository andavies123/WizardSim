namespace GameWorld.Characters.Enemies.EnemyTypes
{
	public class Slime : Enemy
	{
		// Slime should move by hopping from tile to tile
		// Slime should multiply when dead
		// Slime should attack at a close range
		// Slime should hop at wizard when attacking

		public SlimeSize Size { get; private set; }

		public void Initialize(SlimeSize size)
		{
			Size = size;
		}

		public void OnDeath()
		{

		}
	}

	public enum SlimeSize
	{
		ExtraLarge, // Breaks into 2 large slimes
		Large, // Breaks into 2 medium slimes
		Medium, // Breaks into 2 small slimes
		Small, // Breaks into 2 extra small slimes
		ExtraSmall // Dies
	}
}
