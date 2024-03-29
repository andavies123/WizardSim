using System;
using UnityEngine;
using Utilities;

namespace Wizards
{
	public class Wizard : MonoBehaviour
	{
		public Guid Id { get; } = Guid.NewGuid();
		public string Name { get; set; }

		private void Awake()
		{
			Name = NameGenerator.GetNewName();
		}
	}
}