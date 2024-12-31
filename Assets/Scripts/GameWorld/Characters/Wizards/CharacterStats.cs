using System;

namespace GameWorld.Characters.Wizards
{
	public class CharacterStats
	{
		public CharacterStats(Func<float> speedValueFormula)
		{
			Speed = new CharacterStat("Speed", speedValueFormula);
		}
		
		public CharacterStat Speed { get; }
	}
}