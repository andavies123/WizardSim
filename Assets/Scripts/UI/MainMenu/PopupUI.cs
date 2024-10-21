using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.MainMenu
{
	public class PopupUI : MonoBehaviour
	{
		[SerializeField, Required] private TMP_Text messageText;
		[SerializeField, Required] private Button acceptButton;
		[SerializeField, Required] private Button rejectButton;
		[SerializeField, Required] private Button cancelButton;
		
		private PopupToken _token;
		
		public void Initialize(PopupToken token, string message, string acceptanceText, string rejectionText)
		{
			_token?.Cancel(); // If we were already assigned a token somehow, we should cancel it    
			_token = token;
			messageText.SetText(message);
			acceptButton.GetComponentInChildren<TMP_Text>().SetText(acceptanceText);
			rejectButton.GetComponentInChildren<TMP_Text>().SetText(rejectionText);
			
			gameObject.SetActive(true);
		}

		private void OnAcceptButtonClicked()
		{
			_token.Accept();
			gameObject.SetActive(false);
		}

		private void OnRejectButtonClicked()
		{
			_token.Reject();
			gameObject.SetActive(false);
		}

		private void OnCancelButtonClicked()
		{
			_token.Cancel();
			gameObject.SetActive(false);
		}

		private void Awake()
		{
			acceptButton.onClick.AddListener(OnAcceptButtonClicked);
			rejectButton.onClick.AddListener(OnRejectButtonClicked);
			cancelButton.onClick.AddListener(OnCancelButtonClicked);
		}

		private void OnDestroy()
		{
			acceptButton.onClick.RemoveAllListeners();
			rejectButton.onClick.RemoveAllListeners();
			cancelButton.onClick.RemoveAllListeners();
		}
	}
}