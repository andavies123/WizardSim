using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using GameWorld.WorldResources;
using MessagingSystem;

namespace Game
{
	public static class Dependencies
	{
		private static readonly ConcurrentDictionary<Type, object> DependencyStore = new();
		private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> KeyedDependencyStore = new();

		static Dependencies()
		{
			TownResourceRepo townResourceRepo = new();
			townResourceRepo.LoadAllTownResources();
			
			// Registering non singletons
			Register(new MessageBroker());
			Register(townResourceRepo);
		}
		
		public static void Register<T>(T dependency)
		{
			if (!DependencyStore.TryAdd(dependency.GetType(), dependency))
				throw new InvalidDataException($"{typeof(T)} dependency already exists");
		}

		public static void Register<T>(T dependency, string key)
		{
			if (!KeyedDependencyStore.TryGetValue(dependency.GetType(), out Dictionary<string, object> dependencyByKey))
			{
				dependencyByKey = new Dictionary<string, object>();
				KeyedDependencyStore.TryAdd(dependency.GetType(), dependencyByKey);
			}

			if (!dependencyByKey.TryAdd(key, dependency))
				throw new InvalidDataException($"{typeof(T)} dependency already exists with key: {key}");
		}

		public static T Get<T>()
		{
			if (!DependencyStore.TryGetValue(typeof(T), out object value))
				throw new InvalidDataException($"{typeof(T)} dependency does not exist");

			return (T)value;
		}

		public static T Get<T>(string key)
		{
			if (!KeyedDependencyStore.TryGetValue(typeof(T), out Dictionary<string, object> dependencyByKey) ||
			    !dependencyByKey.TryGetValue(key, out object value))
				throw new InvalidDataException($"{typeof(T)} dependency not found with key: {key}");

			return (T) value;
		}
	}
}