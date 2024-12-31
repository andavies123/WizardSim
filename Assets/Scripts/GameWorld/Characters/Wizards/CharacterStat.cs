using System;

namespace GameWorld.Characters.Wizards
{
	public class CharacterStat
	{
		private readonly Func<float> _valueFormula;

		public CharacterStat(string statName, Func<float> valueFormula)
		{
			StatName = statName ?? throw new ArgumentNullException(nameof(statName));
			_valueFormula = valueFormula ?? throw new ArgumentNullException(nameof(valueFormula));
		}

		public string StatName { get; }
		public float Value => _valueFormula.Invoke();

		public override string ToString() => $"{StatName}: {Value}";
	}
}