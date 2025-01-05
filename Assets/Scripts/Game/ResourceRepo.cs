using Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Game
{
	public class ResourceRepo<TResource>
	{
		private readonly Dictionary<string, TResource> _resources = new();
		private readonly Func<TResource, string> _keyGetter;
		private readonly string _typeName;

		public ResourceRepo(Func<TResource, string> keyGetter, string typeName)
		{
			_keyGetter = keyGetter;
			_typeName = typeName;
		}

		public IReadOnlyDictionary<string, TResource> Repo => _resources;

		public void Load(string filePath)
		{
			StringBuilder stringBuilder = new($"Loading from {filePath}\n");
			bool errorFound = false;

			TextAsset file = Resources.Load<TextAsset>(filePath);
			if (file != null)
			{
				stringBuilder.AppendLine("<color=green>File loaded successfully</color>");
				List<TResource> resources = JsonConvert.DeserializeObject<List<TResource>>(file.text);

				if (resources.Count > 0)
				{
					foreach (TResource resource in resources)
					{
						string key = _keyGetter?.Invoke(resource);

						if (key.IsNullOrWhiteSpace())
						{
							stringBuilder.AppendLine($"<color=red>Error loading {_typeName}...</color>");
							errorFound = true;
							continue;
						}

						if (!_resources.TryAdd(key, resource))
						{
							stringBuilder.AppendLine($"<color=red>\"{key}\" already exists...</color>");
							errorFound = true;
							continue;
						}

						stringBuilder.AppendLine($"<color=green>Successfully loaded \"{key}\"</color>");
					}
				}
				else
				{
					stringBuilder.AppendLine($"No {_typeName} found...");
					errorFound = true;
				}
			}
			else
			{
				stringBuilder.AppendLine("<color=red>Loading failed...</color>");
				errorFound = true;
			}

			stringBuilder.AppendLine("-----------------------------------------------------------------------------------------");
			stringBuilder.AppendLine($"Loaded {_resources.Count} {_typeName}s");

			if (errorFound)
				Debug.LogError(stringBuilder.ToString());
			else
				Debug.Log(stringBuilder.ToString());
		}
	}
}