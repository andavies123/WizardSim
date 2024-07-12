using System.Collections;
using System.Collections.Generic;

namespace Extensions
{
	public static class CollectionExtensions
	{
		public static bool IsEmpty(this ICollection collection) => collection.Count == 0;
		public static bool IsNotEmpty(this ICollection collection) => collection.Count > 0;

		public static bool IsNullOrEmpty(this ICollection collection) => collection == null || collection.Count == 0;

		/// <summary>
		/// Tries to get the last element of this collection.
		/// Returns a default value if collection is empty
		/// </summary>
		/// <param name="collection">The extended collection</param>
		/// <param name="last">The last element of the collection. Default if collection is empty</param>
		/// <typeparam name="T">The generic type of the collection</typeparam>
		/// <returns>True if an element was found, false if not</returns>
		public static bool TryGetLast<T>(this IList<T> collection, out T last)
		{
			if (collection.Count == 0)
			{
				last = default;
				return false;
			}
			
			last = collection[^1];
			return true;
		}

		/// <summary>
		/// Removes the last element of a collection and returns it via the <see cref="last"/> out variable
		/// </summary>
		/// <param name="collection">The extended collection</param>
		/// <param name="last">The last element of the collection that was removed. Default if collection is empty</param>
		/// <typeparam name="T">The generic type of the collection</typeparam>
		/// <returns>True if an element was removed. False if not</returns>
		public static bool TryRemoveLast<T>(this IList<T> collection, out T last)
		{
			if (collection.Count == 0)
			{
				last = default;
				return false;
			}

			last = collection[^1];
			collection.RemoveAt(collection.Count - 1);
			return true;
		}
	}
}