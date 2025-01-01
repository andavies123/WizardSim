using System;

namespace GameWorld.Characters
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