namespace GameWorld.Characters.Wizards
{
	/// <summary>
	/// Describes general stats related to wizards
	/// </summary>
	public class WizardStats
	{
		public WizardStats(WizardAttributes attributes)
		{
			AttackSpeed = new CharacterStat("Atk Spd",
				() => 0.5f + (attributes.Endurance.CurrentLevel - 1) * 0.1f);
			AttackDamage = new CharacterStat("Atk Dmg",
				() => 2 + (attributes.Strength.CurrentLevel - 1) * 2f);
			
			RockAttackSpeed = new CharacterStat("Rock Atk Spd",
				() => 0.5f + (attributes.Endurance.CurrentLevel - 1) * 0.1f);
			RockAttackDamage = new CharacterStat("Rock Atk Dmg",
				() => 2 + (attributes.Strength.CurrentLevel - 1) * 2f);
		}
		
		// Attack Stats
		public CharacterStat AttackSpeed { get; } // Attacks per second
		public CharacterStat AttackDamage { get; } // Damage dealt per attack
		
		// Rock Stats
		public CharacterStat RockAttackSpeed { get; } // Attacks per second
		public CharacterStat RockAttackDamage { get; } // Damage dealt per attack
	}
}