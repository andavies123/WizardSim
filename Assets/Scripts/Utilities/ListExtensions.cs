using System.Collections;

namespace Utilities
{
	public static class ListExtensions
	{
		public static bool IsEmpty(this IList list) => list.Count == 0;
	}
}