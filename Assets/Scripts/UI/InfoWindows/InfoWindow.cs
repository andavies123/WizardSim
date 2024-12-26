using System.Collections.Generic;
using System.ComponentModel;
using Extensions;
using TMPro;
using UnityEngine;
using Utilities.Attributes;

namespace UI.InfoWindows
{
	public class InfoWindow : MonoBehaviour
	{
		[Header("UI Components")]
		[SerializeField, Required] private TMP_Text titleText;
		[SerializeField, Required] private TMP_Text infoText;

		[Header("Display Settings")]
		[SerializeField] private float infoTextLineHeight = 30f;

		private Interactable _currentInteractable;
		
		public bool IsOpen { get; private set; }

		public void OpenWindow(Interactable interactable)
		{
			DiscardCurrentInteractable();
			ProcessNewCurrentInteractable(interactable);
			gameObject.SetActive(true);
			IsOpen = true;
		}

		public void CloseWindow()
		{
			gameObject.SetActive(false);
			DiscardCurrentInteractable();
			IsOpen = false;
		}

		private void DiscardCurrentInteractable()
		{
			if (!_currentInteractable)
				return;

			_currentInteractable.PropertyChanged -= OnCurrentInteractablePropertyChanged;
			_currentInteractable.IsSelected = false;
			_currentInteractable = null;
		}

		private void ProcessNewCurrentInteractable(Interactable interactable)
		{
			if (!interactable)
				return;

			_currentInteractable = interactable;
			_currentInteractable.PropertyChanged += OnCurrentInteractablePropertyChanged;
			SetTitleText(_currentInteractable.TitleText);
			SetInfoText(_currentInteractable.InfoText);
		}

		private void SetTitleText(string text) => titleText.SetText(text);
		private void SetInfoText(IReadOnlyCollection<string> text)
		{
			infoText.SetText(string.Join('\n', text));
			infoText.rectTransform.SetHeight(text.Count * infoTextLineHeight);
		}

		private void OnCurrentInteractablePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(Interactable.TitleText):
					SetTitleText(_currentInteractable.TitleText);
					break;
				case nameof(Interactable.InfoText):
					SetInfoText(_currentInteractable.InfoText);
					break;
			}
		}
	}
}