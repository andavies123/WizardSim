using System;
using UnityEngine;

namespace Wizards
{
	public class Wizard : MonoBehaviour
	{
		public Guid Id { get; } = Guid.NewGuid();
		public string Name { get; set; } = "Andrew Davies";
	}
}