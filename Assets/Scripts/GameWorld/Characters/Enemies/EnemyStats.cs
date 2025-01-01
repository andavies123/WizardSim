namespace GameWorld.Characters.Enemies
{
	/// <summary>
	/// Describes general stats related to enemies
	/// </summary>
	public class EnemyStats
	{
		public EnemyStats(EnemyAttributes attributes)
		{
			AttackSpeed = new CharacterStat("Atk Spd",
				() => 0.5f + (attributes.Endurance.CurrentLevel - 1) * 0.1f);
			AttackDamage = new CharacterStat("Atk Dmg",
				() => 2 + (attributes.Strength.CurrentLevel - 1) * 2f);
		}
		
		// Attack Stats
		public CharacterStat AttackSpeed { get; } // Attacks per second
		public CharacterStat AttackDamage { get; } // Damage dealt per attack
	}
}