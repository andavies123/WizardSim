using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	public static class WorldObjectPropertiesLoader
	{
		private const string JsonFolderName = "World Object Properties";

		public static readonly Dictionary<string, WorldObjectProperties> WorldObjectPropertiesMap = new();
        
		[RuntimeInitializeOnLoadMethod]
		public static void LoadWorldObjectProperties()
		{
			StringBuilder loggerStringBuilder = new($"Loading World Objects Properties from \"Resources/{JsonFolderName}\"...\n");
			TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>(JsonFolderName);

			if (jsonFiles.Length <= 0)
			{
				Debug.LogWarning($"No {nameof(WorldObjectProperties)} found in \"Resources/{JsonFolderName}\"");
				return;
			}
			
			loggerStringBuilder.AppendLine($"Found {jsonFiles.Length} {nameof(WorldObjectProperties)} in \"Resources/{JsonFolderName}\"\n");

			foreach (TextAsset jsonFile in jsonFiles)
			{
				try
				{
					WorldObjectProperties worldObjectProperties = JsonConvert.DeserializeObject<WorldObjectProperties>(jsonFile.text);
					WorldObjectPropertiesMap.Add(worldObjectProperties.ItemName, worldObjectProperties);
					loggerStringBuilder.AppendLine($"Loaded {worldObjectProperties.ItemName}");
				}
				catch (Exception _)
				{
					loggerStringBuilder.AppendLine($"Unable to load {jsonFile.name}");
				}
			}
			
			Debug.Log(loggerStringBuilder.ToString());
		}
	}
}