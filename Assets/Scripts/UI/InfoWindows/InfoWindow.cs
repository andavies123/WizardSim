using System.Collections.Generic;
using System.ComponentModel;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.InfoWindows
{
	public class InfoWindow : MonoBehaviour
	{
		[Header("UI Components")]
		[SerializeField, Required] private TMP_Text titleText;
		[SerializeField, Required] private TMP_Text infoText;
		[SerializeField, Required] private Button moreInfoButton;

		[Header("Display Settings")]
		[SerializeField] private float infoTextLineHeight = 30f;

		private Interactable _currentInteractable;
		private TMP_Text _moreInfoButtonText;
		private bool _isShowingMoreInfo;
		
		public bool IsOpen { get; private set; }

		private void Awake()
		{
			_moreInfoButtonText = moreInfoButton.GetComponentInChildren<TMP_Text>();
		}

		private void OnEnable()
		{
			moreInfoButton.onClick.AddListener(OnMoreInfoButtonPressed);
		}

		private void OnDisable()
		{
			moreInfoButton.onClick.RemoveListener(OnMoreInfoButtonPressed);
		}

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
		private void SetMoreInfoButtonText(string text) => _moreInfoButtonText.SetText(text);

		private void OnMoreInfoButtonPressed()
		{
			_isShowingMoreInfo = !_isShowingMoreInfo;
			
			if (_isShowingMoreInfo)
			{
				SetInfoText(_currentInteractable.ExtendedInfoText);
				SetMoreInfoButtonText("^");
			}
			else
			{
				SetInfoText(_currentInteractable.InfoText);
				SetMoreInfoButtonText("v");
			}
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