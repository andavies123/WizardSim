using System;
using UnityEngine;

namespace Extensions
{
	public static class VectorExtensions
	{
		// Vector2 Extensions
		public static Vector3 ToVector3(this Vector2 vector2, VectorSub vectorSub, float sub = 0) => vectorSub switch
			{
				VectorSub.SubXY => new Vector3(sub, vector2.x, vector2.y),
				VectorSub.SubYX => new Vector3(sub, vector2.y, vector2.x),
				VectorSub.XSubY => new Vector3(vector2.x, sub, vector2.y),
				VectorSub.YSubX => new Vector3(vector2.y, sub, vector2.x),
				VectorSub.XYSub => new Vector3(vector2.x, vector2.y, sub),
				VectorSub.YXSub => new Vector3(vector2.y, vector2.x, sub),
				_ => throw new ArgumentOutOfRangeException(nameof(vectorSub), vectorSub, null)
			};
		
		public static Vector3Int ToVector3Int(this Vector2 vector2, VectorSub vectorSub, int sub = 0) => vectorSub switch
		{
			VectorSub.SubXY => new Vector3Int(sub, (int)vector2.x, (int)vector2.y),
			VectorSub.SubYX => new Vector3Int(sub, (int)vector2.y, (int)vector2.x),
			VectorSub.XSubY => new Vector3Int((int)vector2.x, sub, (int)vector2.y),
			VectorSub.YSubX => new Vector3Int((int)vector2.y, sub, (int)vector2.x),
			VectorSub.XYSub => new Vector3Int((int)vector2.x, (int)vector2.y, sub),
			VectorSub.YXSub => new Vector3Int((int)vector2.y, (int)vector2.x, sub),
			_ => throw new ArgumentOutOfRangeException(nameof(vectorSub), vectorSub, null)
		};
	
		// Vector2Int extensions
		public static Vector3 ToVector3(this Vector2Int vector2, VectorSub vectorSub, float sub = 0) => vectorSub switch
		{
			VectorSub.SubXY => new Vector3(sub, vector2.x, vector2.y),
			VectorSub.SubYX => new Vector3(sub, vector2.y, vector2.x),
			VectorSub.XSubY => new Vector3(vector2.x, sub, vector2.y),
			VectorSub.YSubX => new Vector3(vector2.y, sub, vector2.x),
			VectorSub.XYSub => new Vector3(vector2.x, vector2.y, sub),
			VectorSub.YXSub => new Vector3(vector2.y, vector2.x, sub),
			_ => throw new ArgumentOutOfRangeException(nameof(vectorSub), vectorSub, null)
		};
		
		public static Vector3Int ToVector3Int(this Vector2Int vector2, VectorSub vectorSub, int sub = 0) => vectorSub switch
		{
			VectorSub.SubXY => new Vector3Int(sub, vector2.x, vector2.y),
			VectorSub.SubYX => new Vector3Int(sub, vector2.y, vector2.x),
			VectorSub.XSubY => new Vector3Int(vector2.x, sub, vector2.y),
			VectorSub.YSubX => new Vector3Int(vector2.y, sub, vector2.x),
			VectorSub.XYSub => new Vector3Int(vector2.x, vector2.y, sub),
			VectorSub.YXSub => new Vector3Int(vector2.y, vector2.x, sub),
			_ => throw new ArgumentOutOfRangeException(nameof(vectorSub), vectorSub, null)
		};
	}

	/// <summary>
	/// Enum to describe the position/order the Vector2 values will be
	/// inserted into the Vector3.
	/// E.g., SubYX would insert into the Vector3 like new Vector3(sub, vector2.y, vector2.x) 
	/// </summary>
	public enum VectorSub
	{
		SubXY, SubYX, XSubY, YSubX, XYSub, YXSub
	}
}