using System;

namespace GameWorld
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