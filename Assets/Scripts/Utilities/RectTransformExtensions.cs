using System;
using UnityEngine;

namespace Utilities
{
	public static class RectTransformExtensions
	{
		public static void SetAnchor(this RectTransform rectTransform, AnchorOption option)
		{
			rectTransform.anchorMin = option switch
			{
				AnchorOption.TopLeft => new Vector2(0, 1),
				AnchorOption.TopCenter => new Vector2(0.5f, 1),
				AnchorOption.TopRight => new Vector2(1, 1),
				AnchorOption.MiddleLeft => new Vector2(0, 0.5f),
				AnchorOption.MiddleCenter => new Vector2(0.5f, 0.5f),
				AnchorOption.MiddleRight => new Vector2(1, 0.5f),
				AnchorOption.BottomLeft => new Vector2(0, 0),
				AnchorOption.BottomCenter => new Vector2(0.5f, 0),
				AnchorOption.BottomRight => new Vector2(1, 0),
				AnchorOption.StretchTop => new Vector2(0, 1),
				AnchorOption.StretchMiddle => new Vector2(0, 0.5f),
				AnchorOption.StretchBottom => new Vector2(0, 0),
				AnchorOption.StretchLeft => new Vector2(0, 0),
				AnchorOption.StretchCenter => new Vector2(0.5f, 0),
				AnchorOption.StretchRight => new Vector2(1, 0),
				AnchorOption.StretchFill => new Vector2(0, 0),
				_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
			};

			rectTransform.anchorMax = option switch
			{
				AnchorOption.TopLeft => new Vector2(0, 1),
				AnchorOption.TopCenter => new Vector2(0.5f, 1),
				AnchorOption.TopRight => new Vector2(1, 1),
				AnchorOption.MiddleLeft => new Vector2(0, 0.5f),
				AnchorOption.MiddleCenter => new Vector2(0.5f, 0.5f),
				AnchorOption.MiddleRight => new Vector2(1, 0.5f),
				AnchorOption.BottomLeft => new Vector2(0, 0),
				AnchorOption.BottomCenter => new Vector2(0.5f, 0),
				AnchorOption.BottomRight => new Vector2(1, 0),
				AnchorOption.StretchTop => new Vector2(1, 1),
				AnchorOption.StretchMiddle => new Vector2(1, 0.5f),
				AnchorOption.StretchBottom => new Vector2(1, 0),
				AnchorOption.StretchLeft => new Vector2(0, 1),
				AnchorOption.StretchCenter => new Vector2(0.5f, 1),
				AnchorOption.StretchRight => new Vector2(1, 1),
				AnchorOption.StretchFill => new Vector2(1, 1),
				_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
			};

			rectTransform.pivot = option switch
			{
				AnchorOption.TopLeft => new Vector2(0, 1),
				AnchorOption.TopCenter => new Vector2(0.5f, 1),
				AnchorOption.TopRight => new Vector2(1, 1),
				AnchorOption.MiddleLeft => new Vector2(0, 0.5f),
				AnchorOption.MiddleCenter => new Vector2(0.5f, 0.5f),
				AnchorOption.MiddleRight => new Vector2(1, 0.5f),
				AnchorOption.BottomLeft => new Vector2(0, 0),
				AnchorOption.BottomCenter => new Vector2(0.5f, 0),
				AnchorOption.BottomRight => new Vector2(1, 0),
				AnchorOption.StretchTop => new Vector2(0.5f, 1),
				AnchorOption.StretchMiddle => new Vector2(0.5f, 0.5f),
				AnchorOption.StretchBottom => new Vector2(0.5f, 0),
				AnchorOption.StretchLeft => new Vector2(0, 0.5f),
				AnchorOption.StretchCenter => new Vector2(0.5f, 0.5f),
				AnchorOption.StretchRight => new Vector2(1, 0.5f),
				AnchorOption.StretchFill => new Vector2(0.5f, 0.5f),
				_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
			};
		}
	}

	public enum AnchorOption
	{
		TopLeft,
		TopCenter,
		TopRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		BottomLeft,
		BottomCenter,
		BottomRight,
		StretchTop,
		StretchMiddle,
		StretchBottom,
		StretchLeft,
		StretchCenter,
		StretchRight,
		StretchFill
	}
}