using UnityEngine;

namespace Extensions
{
	public static class RectTransformExtensions
	{
		public static void SetSize(this RectTransform rectTransform, Vector2 size) =>
			rectTransform.sizeDelta = size;
		
		public static void SetWidth(this RectTransform rectTransform, float width) =>
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
		
		public static void SetHeight(this RectTransform rectTransform, float height) =>
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
	}
}