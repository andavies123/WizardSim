using System;

namespace GameWorld.Characters
{
	public class CharacterEventArgs : EventArgs
	{
		public CharacterEventArgs(Character character)
		{
			Character = character;
		}
		
		public Character Character { get; }
	}
}