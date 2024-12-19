using System.Collections;
using System.Collections.Generic;

namespace Extensions
{
	public static class CollectionExtensions
	{
		public static bool IsEmpty<T>(this List<T> list) => list.Count == 0;
		public static bool IsNotEmpty<T>(this List<T> list) => list.Count > 0;
		public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;
		
		public static bool IsEmpty(this ICollection collection) => collection.Count == 0;
		public static bool IsNotEmpty(this ICollection collection) => collection.Count > 0;
		public static bool IsNullOrEmpty(this ICollection collection) => collection == null || collection.Count == 0;
		
		public static bool IsEmpty<T>(this ICollection<T> collection) => collection.Count == 0;
		public static bool IsNotEmpty<T>(this ICollection<T> collection) => collection.Count > 0;
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;

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

		public static bool TryRemoveFirst<T>(this IList<T> collection, out T first)
		{
			if (collection.IsEmpty())
			{
				first = default;
				return false;
			}

			first = collection[0];
			collection.RemoveAt(0);
			return true;
		}

		/// <summary>
		/// Checks if a given index is valid for this collection
		/// </summary>
		/// <param name="list">The extended list</param>
		/// <param name="index">The index to check to see if it is valid</param>
		/// <returns>True if <see cref="index"/> is a valid index. False if not.</returns>
		public static bool IsValidIndex(this IList list, int index)
		{
			return index >= 0 && index < list.Count;
		}
	}
}