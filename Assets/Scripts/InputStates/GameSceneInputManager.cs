using Game;
using UnityEngine;

namespace InputStates
{
	public class GameSceneInputManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;

		[Header("Input States")]
		[SerializeField] private GameplayInputState gameplayInputState;
		[SerializeField] private PauseMenuInputState pauseMenuInputState;

		private InputState _activeInputState;
		
		private void Awake()
		{
			gameManager.GamePaused += OnGamePaused;
			gameManager.GameResumed += OnGameResumed;
			
			SetActiveInputState(gameplayInputState);
		}

		private void OnDestroy()
		{
			gameManager.GamePaused -= OnGamePaused;
			gameManager.GameResumed -= OnGameResumed;
		}
		
		private void OnGamePaused() => SetActiveInputState(pauseMenuInputState);
		private void OnGameResumed() => SetActiveInputState(gameplayInputState);

		private void SetActiveInputState(InputState inputState)
		{
			if (_activeInputState != null)
				_activeInputState.DisableInputs();

			_activeInputState = inputState;
			_activeInputState.EnableInputs();
		}
	}
}