using Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputStates
{
	public class GameSceneInputManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		[SerializeField] private PlayerInput playerInput;

		[Header("Input States")]
		[SerializeField] private GameplayInputState gameplayInputState;
		[SerializeField] private PauseMenuInputState pauseMenuInputState;
		
		private void Awake()
		{
			gameManager.GamePaused += OnGamePaused;
			gameManager.GameResumed += OnGameResumed;
		}

		private void OnDestroy()
		{
			gameManager.GamePaused -= OnGamePaused;
			gameManager.GameResumed -= OnGameResumed;
		}
		
		private void OnGamePaused() => SetActiveInputState(pauseMenuInputState.actionMapName);
		private void OnGameResumed() => SetActiveInputState(gameplayInputState.actionMapName);

		private void SetActiveInputState(string actionMapName) => playerInput.SwitchCurrentActionMap(actionMapName);
	}
}