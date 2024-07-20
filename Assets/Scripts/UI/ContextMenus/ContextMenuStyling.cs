using System;
using UnityEngine;

namespace UI.ContextMenus
{
	[Serializable]
	public class ContextMenuStyling
	{
		[field: Header("Background Colors")]
		[field: SerializeField] public Color DefaultBackgroundColor { get; private set; } = Color.black;
		[field: SerializeField] public Color DisabledBackgroundColor { get; private set; } = Color.gray;
		[field: SerializeField] public Color FocusedBackgroundColor { get; private set; } = Color.blue;
		
		[field: Header("Text Colors")]
		[field: SerializeField] public Color DefaultTextColor { get; private set; } = Color.white;
		[field: SerializeField] public Color DisabledTextColor { get; private set; } = Color.white;
	}
}