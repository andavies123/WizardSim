using System.Collections;
using Challenges;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Attributes;

namespace UI.Challenges
{
	// Todo: Clean up how often the UI gets updated. Split up each update into separate methods then call individually
	public class ChallengesUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[Header("UI Components")]
		[SerializeField, Required] private Canvas canvas;
		[SerializeField, Required] private TMP_Text nameText;
		[SerializeField, Required] private TMP_Text progressText;
		
		[Header("Description UI Components")]
		[SerializeField, Required] private Transform descriptionGroup;
		[SerializeField, Required] private TMP_Text descriptionText;

		[Header("External Components")]
		[SerializeField, Required] private ChallengesManager challengesManager;

		private bool _showDescription;

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

			descriptionText.SetText(challenge.Description);
			descriptionGroup.gameObject.SetActive(_showDescription);
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

		public void OnPointerEnter(PointerEventData eventData)
		{
			_showDescription = true;
			UpdateUI();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_showDescription = false;
			UpdateUI();
		}
	}
}