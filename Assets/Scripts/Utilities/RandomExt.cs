using UnityEngine;

namespace Utilities
{
	public static class RandomExt
	{
		public static Vector3 RangeVector3(
			float minXInclusive, float maxXInclusive,
			float minYInclusive, float maxYInclusive,
			float minZInclusive, float maxZInclusive) => new(
				Random.Range(minXInclusive, maxXInclusive),
				Random.Range(minYInclusive, maxYInclusive),
				Random.Range(minZInclusive, maxZInclusive));

		public static Vector3 RangeVector3(float minInclusive, float maxInclusive) => RangeVector3(
				minInclusive, maxInclusive,
				minInclusive, maxInclusive,
				minInclusive, maxInclusive);

		public static Vector3Int RangeVector3Int(
			int minXInclusive, int maxXExclusive,
			int minYInclusive, int maxYExclusive,
			int minZInclusive, int maxZExclusive) => new(
				Random.Range(minXInclusive, maxXExclusive),
				Random.Range(minYInclusive, maxYExclusive),
				Random.Range(minZInclusive, maxZExclusive));

		public static Vector3Int RangeVector3Int(int minInclusive, int maxExclusive) => RangeVector3Int(
				minInclusive, maxExclusive,
				minInclusive, maxExclusive,
				minInclusive, maxExclusive);
		
		public static Vector2 RangeVector2(
			float minXInclusive, float maxXInclusive,
			float minYInclusive, float maxYInclusive) => new(
			Random.Range(minXInclusive, maxXInclusive),
			Random.Range(minYInclusive, maxYInclusive));

		public static Vector2 RangeVector2(float minInclusive, float maxInclusive) => RangeVector2(
			minInclusive, maxInclusive,
			minInclusive, maxInclusive);

		public static Vector2Int RangeVector2Int(
			int minXInclusive, int maxXExclusive,
			int minYInclusive, int maxYExclusive) => new(
			Random.Range(minXInclusive, maxXExclusive),
			Random.Range(minYInclusive, maxYExclusive));

		public static Vector2Int RangeVector2Int(int minInclusive, int maxExclusive) => RangeVector2Int(
			minInclusive, maxExclusive,
			minInclusive, maxExclusive);
	}
}