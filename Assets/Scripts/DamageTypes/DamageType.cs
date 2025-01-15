using Extensions;
using System.Collections.Generic;
using UnityEngine;

public struct DamageType
{
	public string Name { get; set; }
	public List<string> StrongAgainst { get; set; }
	public List<string> WeakAgainst { get; set; }
	public List<float> DamageTextRGBA { get; set; }


	private Color? _damageTextColor;
	public Color DamageTextColor => _damageTextColor ??= DamageTextRGBA.ToColor();
}