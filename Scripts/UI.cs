using Godot;
using System;

public partial class UI : CanvasLayer {
	public static UI Instance { get; private set; }
	[Signal] 
	public delegate void GamePausedEventHandler(bool isPaused);
	[Export] private Button _continueButton;
	[Export] private Button _mainMenuButton;
	[Export] private Button _quitButton;
	[Export] private MarginContainer _pauseMenu;
	[Export] private MarginContainer _loadingScreen;
	[Export] private MarginContainer _interactableMessage;
	public override void _Ready() {
		Instance = this;
		_continueButton.Pressed += ContinueButtonPressed;
		_mainMenuButton.Pressed += MainMenuButtonPressed;
		_quitButton.Pressed += QuitButtonPressed;
		HidePauseMenu();
		HideLoadingScreen();
	}

	public void ShowPauseMenu() {
		_pauseMenu.Show();
		EmitSignal(SignalName.GamePaused, true);
		GetTree().Paused = true;
	}

	public void HidePauseMenu() {
		_pauseMenu.Hide();
		EmitSignal(SignalName.GamePaused, false);
	}

	public void ShowLoadingScreen() {
		_loadingScreen.Show();
	}

	public void HideLoadingScreen() {
		_loadingScreen.Hide();
	}

	public void ShowInteractableMessage() {
		_interactableMessage.Visible = true;
	}

	public void HideInteractableMessage() {
		_interactableMessage.Visible = false;
	}
	
	private void ContinueButtonPressed() {
		HidePauseMenu();
		GetTree().Paused = false;
		PlayerController.Instance.ChangePauseMenuShown(false);
		PlayerController.Instance.ChangeMouseCapturing();
	}
	private void MainMenuButtonPressed() {
		HidePauseMenu();
		GetTree().Paused = false;
		HideInteractableMessage();
		PlayerController.Instance.ChangePauseMenuShown(false);
		GameManager.Instance.GoToLevel(0);
	}

	private void QuitButtonPressed() {
		GetTree().Quit();
	}
}