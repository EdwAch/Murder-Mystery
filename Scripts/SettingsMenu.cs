using Godot;
using System;

public partial class SettingsMenu : MarginContainer {
	public static SettingsMenu Instance { get; private set; }
	[Export] private HSlider _sensitivitySlider;
	[Export] private MarginContainer _confirmationPopup;
	[Export] private MarginContainer _inputKeyContainer;
	[Export] private Button _interactionButton;
	[Export] private Button _backButton;
	[Export] private Button _defaultButton;
	[Export] private Button _confirmationYesButton;
	[Export] private Button _confirmationNoButton;
	[Export] private Button _confirmationReturnButton;
	[Export] private Button _inputOkButton;
	[Export] private RichTextLabel _confirmationText;
	private string[] _confirmationType = {"Default", "Change"};
	private bool _returnValuesToDefault = false;
	private const float BaseSensitivity = 0.003f;
	private float _newSensitivity;
	private float _playerChosenSensitivity;
	private bool _settingsChanged = false;
	private bool _awaitingKeyInput = false;
	public override void _Ready() {
		Instance = this;
		_sensitivitySlider.ValueChanged += OnSensitivitySliderValueChanged;
		_backButton.Pressed += BackButtonPressed;
		_defaultButton.Pressed += DefaultButtonPressed;
		_confirmationYesButton.Pressed += ConfirmationYesButtonPressed;
		_confirmationNoButton.Pressed += ConfirmationNoButtonPressed;
		_confirmationReturnButton.Pressed += ConfirmationReturnButtonPressed;
		_inputOkButton.Pressed += InputOkButtonPressed;
	}

	private void OnSensitivitySliderValueChanged(double value) {
		_newSensitivity = BaseSensitivity * ((float)value / 50.0f);
		if (_playerChosenSensitivity == _newSensitivity || BaseSensitivity == _newSensitivity) {
			_settingsChanged = false;
		} else {
			_settingsChanged = true;
		}
	}

	private void ShowConfirmationPopup(bool value, string str) {
		if (str == "Default") {
			ShowConfirmationReturnButton(false);
			_returnValuesToDefault = true;
			_confirmationText.Text = "Are you sure you want to restore default settings?";
		} else {
			ShowConfirmationReturnButton(true);
			_returnValuesToDefault = false;
			_confirmationText.Text = "You have changed certain settings. Are you sure you want to save these changes?";
		}
		_confirmationPopup.Visible = value;
	}

	private void BackButtonPressed() {
		if (_awaitingKeyInput) {
			ShowInputKeyContainer(true);
		} else if (_settingsChanged) {
			ShowConfirmationPopup(true, _confirmationType[1]);
		} else {
			UI.Instance.ShowSettingsMenu(false);
			UI.Instance.ShowMainMenu(true);
		}
	}

	private void DefaultButtonPressed() {
		if (_awaitingKeyInput) {
			ShowInputKeyContainer(true);
		} else {
			ShowConfirmationPopup(true, _confirmationType[0]);
		}
	}

	private void ConfirmationYesButtonPressed() {
		if (_returnValuesToDefault) {
			ReturnValuesToDefault();
		} else {
			UpdateSettings();
		}
		ShowConfirmationPopup(false, _confirmationType[1]);
		UI.Instance.ShowSettingsMenu(false);
		UI.Instance.ShowMainMenu(true);
	}
	
	private void ConfirmationNoButtonPressed() {
		if (!_returnValuesToDefault) {
			ReturnValuesToDefault();
		}
		ShowConfirmationPopup(false, _confirmationType[1]);
		UI.Instance.ShowSettingsMenu(false);
		UI.Instance.ShowMainMenu(true);
	}

	private void ConfirmationReturnButtonPressed() {
		ShowConfirmationPopup(false, _confirmationType[1]);
	}

	private void InputOkButtonPressed() {
		ShowInputKeyContainer(false);
	}

	private void ShowConfirmationReturnButton(bool value) {
		_confirmationReturnButton.Visible = value;
	}

	private void ReturnValuesToDefault() {
		_newSensitivity = BaseSensitivity;
		PlayerController.Instance.ChangeSensitivity(BaseSensitivity);
		_sensitivitySlider.Value = 50;
		ReturnKeyToDefault("Interact", Key.E);
		_interactionButton.Text = "E";
		InteractablePromptKeyGetting.Instance.NewKeyUsed("E");
		_settingsChanged = false;
	}

	private void UpdateSettings() {
		if (_playerChosenSensitivity != 0) {
			_playerChosenSensitivity = _newSensitivity;
			PlayerController.Instance.ChangeSensitivity(_newSensitivity);
		}
		_settingsChanged = false;
	}

	private void ReturnKeyToDefault(StringName name, Key key) {
		InputMap.ActionEraseEvents(name);

		var physicalEvent = new InputEventKey{
			PhysicalKeycode = key
		};
		InputMap.ActionAddEvent(name, physicalEvent);
	}

	private void ShowInputKeyContainer(bool value) {
		_inputKeyContainer.Visible = value;
	}

	public void SettingsChanged() {
		_settingsChanged = true;
	}

	public void AwaitingKeyInput(bool value) {
		_awaitingKeyInput = value;
	}
}