using System.Diagnostics;
using Cysharp.Threading.Tasks;
using Game.EventChannels;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace PersistantManagers
{
	public class SceneLoader : MonoBehaviour
	{
		[SerializeField] private StringEventChannel sceneChangeRequestChannel; 
		
		private static async void OnSceneChangeRequested(object sender, string sceneName)
		{
			if (SceneManager.GetSceneByName(sceneName).isLoaded)
				return;

			LogSceneRequest(sceneName, sender.ToString());
			await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}
		
		private void OnEnable()
		{
			sceneChangeRequestChannel.Raised += OnSceneChangeRequested;
		}

		private void OnDisable()
		{
			sceneChangeRequestChannel.Raised -= OnSceneChangeRequested;
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogSceneRequest(string sceneName, string requester)
		{
			Debug.Log($"Changing scene to \"{sceneName}\" requested by \"{requester}\"");
		}
	}
}