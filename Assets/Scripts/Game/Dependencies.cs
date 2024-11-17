using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using MessagingSystem;

namespace Game
{
	public static class Dependencies
	{
		private static readonly ConcurrentDictionary<Type, object> DependencyStore = new();
		private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> KeyedDependencyStore = new();

		static Dependencies()
		{
			// Registering non Mono-Behaviour Dependencies here
			RegisterDependency(new MessageBroker());
		}
		
		public static void RegisterDependency<T>(T dependency)
		{
			if (!DependencyStore.TryAdd(dependency.GetType(), dependency))
				throw new InvalidDataException($"{typeof(T)} dependency already exists");
		}

		public static void RegisterDependency<T>(T dependency, string key)
		{
			if (!KeyedDependencyStore.TryGetValue(dependency.GetType(), out Dictionary<string, object> dependencyByKey))
			{
				dependencyByKey = new Dictionary<string, object>();
				KeyedDependencyStore.TryAdd(dependency.GetType(), dependencyByKey);
			}

			if (!dependencyByKey.TryAdd(key, dependency))
				throw new InvalidDataException($"{typeof(T)} dependency already exists with key: {key}");
		}

		public static T GetDependency<T>()
		{
			if (!DependencyStore.TryGetValue(typeof(T), out object value))
				throw new InvalidDataException($"{typeof(T)} dependency does not exist");

			return (T)value;
		}

		public static T GetDependency<T>(string key)
		{
			if (!KeyedDependencyStore.TryGetValue(typeof(T), out Dictionary<string, object> dependencyByKey) ||
			    !dependencyByKey.TryGetValue(key, out object value))
				throw new InvalidDataException($"{typeof(T)} dependency not found with key: {key}");

			return (T) value;
		}
	}
}