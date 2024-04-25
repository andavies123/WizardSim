using System;
using System.Collections.Concurrent;
using System.IO;
using InputStates;

namespace Game
{
	public static class Dependencies
	{
		private static readonly ConcurrentDictionary<Type, object> DependencyStore = new();
		
		static Dependencies()
		{
			RegisterDependency(new GameplayInput());
			RegisterDependency(new InteractionInput());
			RegisterDependency(new PauseMenuInput());
		}
		
		public static void RegisterDependency<T>(T dependency)
		{
			if (!DependencyStore.TryAdd(dependency.GetType(), dependency))
				throw new InvalidDataException($"{typeof(T)} dependency already exists");
		}

		public static T GetDependency<T>()
		{
			if (!DependencyStore.TryGetValue(typeof(T), out object value))
				throw new InvalidDataException($"{typeof(T)} dependency does not exist");

			return (T)value;
		}
	}
}