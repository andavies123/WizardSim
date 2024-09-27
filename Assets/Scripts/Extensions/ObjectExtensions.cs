using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Checks the extended object if its null then throws an exception if it is
		/// </summary>
		/// <param name="obj">The extended object that will be checked for null</param>
		/// <param name="objName">The name of the extended object. Used for exception message</param>
		/// <typeparam name="T">The type of the object that is passed so it can be returned as the same type</typeparam>
		/// <returns>The original object if it is not null. Used to make these checks one liners</returns>
		/// <exception cref="NullReferenceException">Thrown if the extended object is null</exception>
		public static T ThrowIfNull<T>(this T obj, string objName) where T : class
		{
			if (obj == null)
				throw new NullReferenceException($"{objName} is null");

			return obj;
		}
		
		/// <summary>
		/// Checks the extended object if it is null, then logs a warning message if it is
		/// </summary>
		/// <param name="obj">The extended object that will be checked for null</param>
		/// <param name="message">Message that will show in the logs</param>
		/// <param name="context">The context object for the warning message</param>
		/// <typeparam name="T">The type of the object that is passed</typeparam>
		/// <returns>True if the object is null, otherwise false</returns>
		public static bool IsNullThenLogWarning<T>(this T obj, string message, Object context) where T : class
		{
			if (obj != null) 
				return false;
			
			Debug.LogWarning(message, context);
			return true;

		}
	}
}