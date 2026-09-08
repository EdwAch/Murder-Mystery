using Godot;
using System;

public partial class SettingsMenu : MarginContainer {
	
	[Export] private HSlider _sensitivitySlider;
	[Export] private MarginContainer _confirmationPopup;
	[Export] private Button _backButton;
	[Export] private Button _confirmationYesButton;
	[Export] private Button _confirmationNoButton;
	[Export] private Button _confirmationReturnButton;
	private const float BaseSensitivity = 0.003f;
	private float _newSensitivity;
	private bool _settingsChanged = false;
	public override void _Ready() {
		_sensitivitySlider.ValueChanged += OnSensitivitySliderValueChanged;
		_backButton.Pressed += BackButtonPressed;
		_confirmationYesButton.Pressed += ConfirmationYesButtonPressed;
		_confirmationNoButton.Pressed += ConfirmationNoButtonPressed;
		_confirmationReturnButton.Pressed += ConfirmationReturnButtonPressed;
	}

	private void OnSensitivitySliderValueChanged(double value) {
		_newSensitivity = BaseSensitivity * ((float)value / 50.0f);
		_settingsChanged = true;
	}

	private void ShowConfirmationPopup(bool value) {
		_confirmationPopup.Visible = value;
	}

	private void BackButtonPressed() {
		if (_settingsChanged) {
			ShowConfirmationPopup(true);
		} else {
			UI.Instance.ShowSettingsMenu(false);
			UI.Instance.ShowMainMenu(true);
		}
	}

	private void ConfirmationYesButtonPressed() {
		ShowConfirmationPopup(false);
		UI.Instance.ShowSettingsMenu(false);
		UI.Instance.ShowMainMenu(true);
	}
	
	private void ConfirmationNoButtonPressed() {
		ShowConfirmationPopup(false);
		UI.Instance.ShowSettingsMenu(false);
		UI.Instance.ShowMainMenu(true);
	}

	private void ConfirmationReturnButtonPressed() {
		ShowConfirmationPopup(false);
	}
}