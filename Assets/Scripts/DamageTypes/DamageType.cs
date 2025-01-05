using Extensions;
using System.Collections.Generic;
using UnityEngine;

public class DamageType
{
	private Color? _damageTextColor = default;

	public string Name { get; set; }
	public List<string> StrongAgainst { get; set; }
	public List<string> WeakAgainst { get; set; }
	public List<float> DamageTextRGBA { get; set; }

	public Color DamageTextColor => _damageTextColor ??= DamageTextRGBA.ToColor();
}