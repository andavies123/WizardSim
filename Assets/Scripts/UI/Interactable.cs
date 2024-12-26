using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameWorld.WorldObjects;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(Rigidbody))]
	public class Interactable : MonoBehaviour, INotifyPropertyChanged
	{
		private bool _isHovered;
		private bool _isSelected;
		private string _titleText = string.Empty;
		private List<string> _infoText = new();

		public event EventHandler PrimaryActionSelected;
		public event EventHandler SecondaryActionSelected;

		public bool IsHovered
		{
			get => _isHovered;
			set => SetValue(ref _isHovered, value);
		}

		public bool IsSelected
		{
			get => _isSelected;
			set => SetValue(ref _isSelected, value);
		}

		public string TitleText
		{
			get => _titleText;
			set => SetValue(ref _titleText, value);
		}

		public List<string> InfoText
		{
			get => _infoText;
			set => SetValue(ref _infoText, value);
		}

		public void SelectPrimaryAction() => PrimaryActionSelected?.Invoke(this, EventArgs.Empty);
		public void SelectSecondaryAction() => SecondaryActionSelected?.Invoke(this, EventArgs.Empty);

		public void InitializeWithProperties(InteractableProperties properties)
		{
			TitleText = properties.Title;
			InfoText = new List<string> {properties.Description};
		}

		public void InitializeWithProperties(InteractableRelatedProperties properties)
		{
			TitleText = properties.Title;
			InfoText = new List<string> {properties.Description};
		}

		#region INotifyPropertyChanged Implementation

		public event PropertyChangedEventHandler PropertyChanged;
		
		private void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void SetValue<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) 
				return;
			
			field = value;
			OnPropertyChanged(propertyName);
		}

		#endregion
	}
}