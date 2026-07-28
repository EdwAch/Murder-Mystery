using Godot;
using System;

public partial class UI : CanvasLayer {
	public static UI Instance { get; private set; }
	[Export] private Button _continueButton;
	[Export] private Button _quitButton;
	[Export] private MarginContainer _pauseMenu;
	public override void _Ready() {
		Instance = this;
		_continueButton.Pressed += ContinueButtonPressed;
		_quitButton.Pressed += QuitButtonPressed;
		HidePauseMenu();
	}

	public void ShowPauseMenu() {
		_pauseMenu.Show();
		GetTree().Paused = true;
	}

	public void HidePauseMenu() {
		_pauseMenu.Hide();
	}
	
	private void ContinueButtonPressed() {
		HidePauseMenu();
		GetTree().Paused = false;
		PlayerController.Instance.ChangePauseMenuShown(false);
		PlayerController.Instance.ChangeMouseCapturing();
	}
	private void QuitButtonPressed() {
		GetTree().Quit();
	}
}