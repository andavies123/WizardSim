using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace UI.InfoWindows
{
	public class InfoWindow : MonoBehaviour
	{
		[Header("UI Components")]
		[SerializeField] private TMP_Text titleText;
		[SerializeField] private TMP_Text infoText;

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
		private void SetInfoText(string text) => infoText.SetText(text);

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