using Extensions;
using GeneralClasses.Health.HealthEventArgs;
using System;
using UnityEngine;

namespace GameWorld.Characters
{
	[RequireComponent(typeof(Character))]
	public class CharacterDeath : MonoBehaviour
	{
		private Character _character;

		public event EventHandler<CharacterDiedEventArgs> Died;

		private void Awake()
		{
			_character = GetComponent<Character>();
		}

		private void Start()
		{
			_character.Health.Health.ReachedMinHealth += OnReachedMinHealth;
		}

		private void OnDestroy()
		{
			_character.Health.Health.ReachedMinHealth -= OnReachedMinHealth;
		}

		private void OnReachedMinHealth(object sender, ReachedMinHealthEventArgs args)
		{
			Died?.Invoke(this, new CharacterDiedEventArgs(_character));
			_character.gameObject.Destroy();
		}
	}

	public class CharacterDiedEventArgs : EventArgs
	{
		public CharacterDiedEventArgs(Character deadCharacter)
		{
			DeadCharacter = deadCharacter;
		}

		public Character DeadCharacter { get; }
	}
}
