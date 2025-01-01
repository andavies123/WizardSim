using System;

namespace GameWorld.Characters.Enemies
{
	/// <summary>
	/// Describes general attributes related to enemies
	/// </summary>
	public class EnemyAttributes
	{
		/// <summary>
		/// Raised when any attribute gets leveled up.
		/// Gives the exact attribute that was leveled up
		/// </summary>
		public event Action<CharacterAttribute> LeveledUp;
        
		public EnemyAttributes()
		{
			Strength.LeveledUp += () => LeveledUp?.Invoke(Strength);
			Endurance.LeveledUp += () => LeveledUp?.Invoke(Endurance);
			Vitality.LeveledUp += () => LeveledUp?.Invoke(Vitality);
			Magic.LeveledUp += () => LeveledUp?.Invoke(Magic);
			Mana.LeveledUp += () => LeveledUp?.Invoke(Mana);
		}
		
		/// <summary>
		/// Affects physical attacks and tasks that require physical work
		/// </summary>
		public CharacterAttribute Strength { get; } = new();

		/// <summary>
		/// Affects movement speed and how long a enemy can go without needing to rest doing physical tasks
		/// </summary>
		public CharacterAttribute Endurance { get; } = new();

		/// <summary>
		/// Affects amount of health and how the enemy is affected by damage
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
	}
}