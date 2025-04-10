using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using Game.EventChannels;
using Game.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Attributes;
using Debug = UnityEngine.Debug;

namespace PersistantManagers
{
	public class SceneLoader : MonoBehaviour
	{
		[SerializeField, Required] private StringEventChannel sceneLoadRequestChannel;
		[SerializeField, Required] private StringEventChannel sceneUnloadRequestChannel;
		
		private static async void OnSceneLoadRequested(object sender, string sceneName)
		{
			if (SceneManager.GetSceneByName(sceneName).isLoaded)
				return;

			LogSceneLoadRequest(sceneName, sender.ToString());
			await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}

		private static async void OnSceneUnloadRequested(object sender, string sceneName)
		{
			if (!SceneManager.GetSceneByName(sceneName).isLoaded)
				return;
			
			LogSceneUnloadRequest(sceneName, sender.ToString());
			await SceneManager.UnloadSceneAsync(sceneName);
		}

		private static void OnCloseGameRequested(object sender, EventArgs args)
		{
			EditorApplication.ExitPlaymode(); // Editor mode
			Application.Quit(); // Application mode
		}
		
		private void OnEnable()
		{
			sceneLoadRequestChannel.Raised += OnSceneLoadRequested;
			sceneUnloadRequestChannel.Raised += OnSceneUnloadRequested;

			GameEvents.General.CloseGame.Requested += OnCloseGameRequested;
		}

		private void OnDisable()
		{
			sceneLoadRequestChannel.Raised -= OnSceneLoadRequested;
			sceneUnloadRequestChannel.Raised -= OnSceneUnloadRequested;
			
			GameEvents.General.CloseGame.Requested -= OnCloseGameRequested;
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogSceneLoadRequest(string sceneName, string requester)
		{
			Debug.Log($"Loading scene \"{sceneName}\" requested by \"{requester}\"");
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogSceneUnloadRequest(string sceneName, string requester)
		{
			Debug.Log($"Unloading scene \"{sceneName}\" requested by \"{requester}\"");
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogActiveSceneSet(string sceneName, string requester)
		{
			Debug.Log($"Setting \"{sceneName}\" as active scene by \"{requester}\"");
		}
	}
}