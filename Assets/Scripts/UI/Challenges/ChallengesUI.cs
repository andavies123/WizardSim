using System.Collections;
using Challenges;
using TMPro;
using UnityEngine;
using Utilities.Attributes;

namespace UI.Challenges
{
	// Todo: UI should display the challenge description when hovering
	// Todo: Challenge progress should get updated periodically
	public class ChallengesUI : MonoBehaviour
	{
		[Header("UI Components")]
		[SerializeField, Required] private Canvas canvas;
		[SerializeField, Required] private TMP_Text nameText;
		[SerializeField, Required] private TMP_Text progressText;

		[Header("External Components")]
		[SerializeField, Required] private ChallengesManager challengesManager;

		private void UpdateUI()
		{
			Challenge challenge = challengesManager.CurrentChallenge;
			if (challenge == null)
			{
				canvas.enabled = false;
				return;
			}

			nameText.SetText(challenge.Name);
			progressText.SetText(challenge.CompletionCriteria.Invoke()
				? "Challenge Completed!"
				: challengesManager.CurrentChallenge.ProgressText?.Invoke());
			canvas.enabled = true;
		}

		private void Awake()
		{
			challengesManager.CurrentChallengeUpdated += OnCurrentChallengeUpdated;
			StartCoroutine(nameof(UpdateProgressText));
		}

		private void OnDestroy()
		{
			challengesManager.CurrentChallengeUpdated -= OnCurrentChallengeUpdated;
			StopAllCoroutines();
		}

		private void OnCurrentChallengeUpdated() => UpdateUI();

		private IEnumerator UpdateProgressText()
		{
			while (true)
			{
				yield return new WaitForSeconds(0.5f);
				UpdateUI();
			}
		}
	}
}