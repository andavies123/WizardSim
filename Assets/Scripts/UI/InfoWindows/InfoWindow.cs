using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
			UpdateInfo();
			SetMoreInfoButtonVisibility(_currentInteractable.ExtendedInfoText.Count > 0);
		}

		private void SetTitleText(string text) => titleText.SetText(text);
		private void SetInfoText(params IReadOnlyCollection<string>[] texts)
		{
			StringBuilder infoTextBuilder = new();
			int totalLines = 0;
			
			foreach (IReadOnlyCollection<string> text in texts)
			{
				totalLines += text.Count;
				foreach (string line in text)
				{
					infoTextBuilder.AppendLine(line);
				}
			}
			
			infoText.SetText(infoTextBuilder);
			infoText.rectTransform.SetHeight(infoTextLineHeight * totalLines);
		}
		
		private void SetMoreInfoButtonText(string text) => _moreInfoButtonText.SetText(text);
		private void SetMoreInfoButtonVisibility(bool visibility) => moreInfoButton.gameObject.SetActive(visibility);

		private void UpdateInfo()
		{
			if (_isShowingMoreInfo)
			{
				SetInfoText(_currentInteractable.InfoText, _currentInteractable.ExtendedInfoText);
				SetMoreInfoButtonText("^");
			}
			else
			{
				SetInfoText(_currentInteractable.InfoText);
				SetMoreInfoButtonText("v");
			}
		}
        
		private void OnMoreInfoButtonPressed()
		{
			_isShowingMoreInfo = !_isShowingMoreInfo;
			UpdateInfo();
		}

		private void OnCurrentInteractablePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(Interactable.TitleText):
					SetTitleText(_currentInteractable.TitleText);
					break;
				case nameof(Interactable.InfoText):
					UpdateInfo();
					break;
				case nameof(Interactable.ExtendedInfoText):
					if (_isShowingMoreInfo)
						UpdateInfo();
					break;
			}
		}
	}
}