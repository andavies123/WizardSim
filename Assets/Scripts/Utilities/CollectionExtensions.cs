﻿using System.Collections;

namespace Utilities
{
	public static class CollectionExtensions
	{
		public static bool IsEmpty(this ICollection collection) => collection.Count == 0;
		public static bool IsNotEmpty(this ICollection collection) => collection.Count > 0;
	}
}