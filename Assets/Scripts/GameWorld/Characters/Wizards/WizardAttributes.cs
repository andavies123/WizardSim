using System;

namespace GameWorld.Characters.Wizards
{
	/// <summary>
	/// Describes general attributes related to wizards
	/// </summary>
	public class WizardAttributes
	{
		/// <summary>
		/// Raised when any attribute gets leveled up.
		/// Gives the exact attribute that was leveled up
		/// </summary>
		public event Action<CharacterAttribute> LeveledUp;
        
		public WizardAttributes()
		{
			Strength.LeveledUp += () => LeveledUp?.Invoke(Strength);
			Endurance.LeveledUp += () => LeveledUp?.Invoke(Endurance);
			Vitality.LeveledUp += () => LeveledUp?.Invoke(Vitality);
			Magic.LeveledUp += () => LeveledUp?.Invoke(Magic);
			Mana.LeveledUp += () => LeveledUp?.Invoke(Mana);
			Intelligence.LeveledUp += () => LeveledUp?.Invoke(Intelligence);
			Courage.LeveledUp += () => LeveledUp?.Invoke(Courage);
		}
		
		/// <summary>
		/// Affects physical attacks and tasks that require physical work
		/// </summary>
		public CharacterAttribute Strength { get; } = new();

		/// <summary>
		/// Affects movement speed and how long long a wizard can go without needing to rest doing physical tasks
		/// </summary>
		public CharacterAttribute Endurance { get; } = new();

		/// <summary>
		/// Affects amount of health and how the wizard is affected by damage
		/// </summary>
		public CharacterAttribute Vitality { get; } = new();

		/// <summary>
		/// Affects magic attacks and tasks that require magic to be completed
		/// </summary>
		public CharacterAttribute Magic { get; } = new();

		/// <summary>
		/// Endurance for magic related attacks
		/// </summary>
		public CharacterAttribute Mana { get; } = new();

		/// <summary>
		/// Affects the ability for wizards to learn new spells and upgrade existing ones.
		/// </summary>
		public CharacterAttribute Intelligence { get; } = new();

		/// <summary>
		/// Affects the fight or flight of a wizard when it comes to facing danger
		/// </summary>
		public CharacterAttribute Courage { get; } = new();
	}
}