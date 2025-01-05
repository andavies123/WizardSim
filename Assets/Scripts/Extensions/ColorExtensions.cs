using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Extensions
{
	public static class ColorExtensions
	{
		public static Color ToColor(this List<float> list)
		{
			if (list == null)
				throw new NullReferenceException(nameof(list));

			if (list.Count == 3)
				return new Color(list[0], list[1], list[2]);

			if (list.Count == 4)
				return new Color(list[0], list[1], list[2], list[3]);

			throw new InvalidDataException($"Collection contains {list.Count} elements. Color requires 3 or 4 elements");
		}
	}
}