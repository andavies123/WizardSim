using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
	public static class NameGenerator
	{
		private static Names s_Names;

		public static string GetNewName()
		{
			if (s_Names == null)
				LoadNames();
			
			return $"{s_Names?.firstNames[Random.Range(0, s_Names?.firstNames.Count ?? 0)]} " +
			       $"{s_Names?.lastNames[Random.Range(0, s_Names?.lastNames.Count ?? 0)]}";
		}

		private static void LoadNames()
		{
			TextAsset jsonFile = Resources.Load<TextAsset>("WizardNames");
			if (jsonFile != null)
			{
				string jsonString = jsonFile.text;
				s_Names = JsonUtility.FromJson<Names>(jsonString);
			}
			else
			{
				Debug.LogError("Failed to load JSON file.");
			}
		}

		[Serializable]
		private class Names
		{
			public List<string> firstNames;
			public List<string> lastNames;
		}
	}
}