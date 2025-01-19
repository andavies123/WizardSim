using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace DamageTypes
{
	public struct DamageType
	{
		public string Name { get; set; }
		public List<string> StrongAgainst { get; set; }
		public List<string> WeakAgainst { get; set; }
		public List<float> DamageTextRgba { get; set; }

		private Color? _damageTextColor;
		public Color DamageTextColor => _damageTextColor ??= DamageTextRgba.ToColor();
	}
}