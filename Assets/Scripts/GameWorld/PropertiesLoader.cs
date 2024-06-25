using System;
using System.Collections.Generic;
using System.Text;
using GameWorld.WorldObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace GameWorld
{
	public static class PropertiesLoader
	{
		private const string WorldObjectPropertiesJsonFolderName = "World Object Properties";
		private const string CharacterPropertiesJsonFolderName = "Character Properties";

		public static readonly Dictionary<string, WorldObjectProperties> WorldObjectPropertiesMap = new();
		public static readonly Dictionary<string, CharacterProperties> CharacterPropertiesMap = new();

		[RuntimeInitializeOnLoadMethod]
		public static void LoadAllProperties()
		{
			LoadProperties(WorldObjectPropertiesJsonFolderName, WorldObjectPropertiesMap);
			LoadProperties(CharacterPropertiesJsonFolderName, CharacterPropertiesMap);
		}

		private static void LoadProperties<T>(string jsonFolderName, IDictionary<string, T> propertiesMap) where T : Properties
		{
			StringBuilder loggerStringBuilder = new($"Loading {typeof(T).Name} from \"Resources/{jsonFolderName}\"...\n");
			TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>(jsonFolderName);

			if (jsonFiles.Length <= 0)
			{
				Debug.LogWarning($"No {typeof(T).Name} found in \"Resources/{jsonFolderName}\"");
				return;
			}
			
			loggerStringBuilder.AppendLine($"Found {jsonFiles.Length} {typeof(T).Name} in \"Resources/{jsonFolderName}\"\n");

			foreach (TextAsset jsonFile in jsonFiles)
			{
				try
				{
					T properties = JsonConvert.DeserializeObject<T>(jsonFile.text);
					propertiesMap.Add(properties.Id, properties);
					loggerStringBuilder.AppendLine($"Loaded {properties.Id}");
				}
				catch (Exception e)
				{
					loggerStringBuilder.AppendLine($"Unable to load {jsonFile.name}");
					loggerStringBuilder.AppendLine(e.ToString());
				}
			}
			
			Debug.Log(loggerStringBuilder.ToString());
		}
	}
}