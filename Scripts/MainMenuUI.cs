using Godot;
using System;

public partial class MainMenuUI : MarginContainer{
	
	[Export] private Button _newGameButton;
	[Export] private Button _continueGameButton;
	[Export] private Button _settingsButton;
	[Export] private Button _quitButton;

    public override void _Ready() {
		CallDeferred(MethodName.SubscribeToSignals);
        _newGameButton.Pressed += NewGameButtonPressed;
		_continueGameButton.Pressed += ContinueButtonPressed;
		_settingsButton.Pressed += SettingsButtonPressed;
		_quitButton.Pressed += QuitButtonPressed;
    }

	private void SubscribeToSignals() {
		GameManager.Instance.InLobby += PlayerInLobby;
		PlayerInLobby(GameManager.Instance.IsInLobby);
	}

	private void PlayerInLobby(bool inLobby) {
		if (inLobby) {
			UI.Instance.ShowMainMenu(inLobby);
		} else {
			UI.Instance.ShowMainMenu(inLobby);
			PlayerController.Instance.ChangeMouseCapturing(true);
		}
	}

	private void NewGameButtonPressed() {
		GameManager.Instance.GoToLevel(1);
	}

	private void ContinueButtonPressed() {
		return;
	}

	private void SettingsButtonPressed() {
		UI.Instance.ShowMainMenu(false);
		UI.Instance.ShowSettingsMenu(true);
	}

	private void QuitButtonPressed() {
		GetTree().Quit();
	}
}