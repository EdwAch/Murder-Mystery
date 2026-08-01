using Godot;
using System;

public partial class MainMenuUI : MarginContainer{
	
	[Export] private Button _newGameButton;
	[Export] private Button _continueGameButton;
	[Export] private Button _settingsButton;
	[Export] private Button _quitButton;

    public override void _Ready() {
        _newGameButton.Pressed += NewGameButtonPressed;
		_continueGameButton.Pressed += ContinueButtonPressed;
		_settingsButton.Pressed += SettingsButtonPressed;
		_quitButton.Pressed += QuitButtonPressed;
    }

	private void NewGameButtonPressed() {
		return;
	}

	private void ContinueButtonPressed() {
		return;
	}

	private void SettingsButtonPressed() {
		return;
	}

	private void QuitButtonPressed() {
		GetTree().Quit();
	}
}