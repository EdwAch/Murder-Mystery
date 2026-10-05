using Godot;
using System;

public partial class UI : CanvasLayer {
	public static UI Instance { get; private set; }
	[Signal] 
	public delegate void GamePausedEventHandler(bool isPaused);
	[Export] private Button _continueButton;
	[Export] private Button _mainMenuButton;
	[Export] private Button _quitButton;
	[Export] private MarginContainer _mainMenu;
	[Export] private MarginContainer _pauseMenu;
	[Export] private MarginContainer _loadingScreen;
	[Export] private MarginContainer _interactableMessage;
	[Export] private MarginContainer _NPCInteraction;
	[Export] private MarginContainer _settingsMenu;
	private bool _wasAwaitingInteraction = false;
	private bool _inNPCInteraction = false;
	private bool _pauseMenuShown = false;
	private bool _inMainMenu = true;
	public override void _Ready() {
		Instance = this;
		_continueButton.Pressed += ContinueButtonPressed;
		_mainMenuButton.Pressed += MainMenuButtonPressed;
		_quitButton.Pressed += QuitButtonPressed;
		HidePauseMenu();
		HideLoadingScreen();
		CallDeferred(MethodName.SubscribeToSignals);
	}

    public override void _Process(double delta) {
        if (!_inMainMenu && Input.IsActionJustPressed("Escape")) {
			if (_pauseMenuShown) {
				HidePauseMenu(); 
				ContinueButtonPressed();
			} else {
				ShowPauseMenu();
				PlayerController.Instance.ChangeMouseCapturing(false);
			}
		}
    }

	private void SubscribeToSignals() {
		NPCInteraction.Instance.InNPCInteraction += PlayerInNPCInteraction;
	}

	private void PlayerInNPCInteraction(bool inInteraction) {
		_inNPCInteraction = inInteraction;
	}

	public void ShowPauseMenu() {
		_pauseMenu.Show();
		if (_interactableMessage.Visible) {
			_wasAwaitingInteraction = true;
		} else {
			_wasAwaitingInteraction = false;
		}
		_pauseMenuShown = true;
		EmitSignal(SignalName.GamePaused, true);
		GetTree().Paused = true;
	}

	public void HidePauseMenu() {
		_pauseMenu.Hide();
		_pauseMenuShown = false;
		EmitSignal(SignalName.GamePaused, false);
	}

	public void ShowLoadingScreen() {
		_loadingScreen.Show();
	}

	public void HideLoadingScreen() {
		_loadingScreen.Hide();
	}

	public void ShowMainMenu(bool value) {
		_mainMenu.Visible = value;
		_inMainMenu = value;
	}

	public void ShowInteractableMessage() {
		_interactableMessage.Visible = true;
	}

	public void HideInteractableMessage() {
		_interactableMessage.Visible = false;
	}

	public void ShowSettingsMenu(bool value) {
		_settingsMenu.Visible = value;
		
	}
	
	private void ContinueButtonPressed() {
		HidePauseMenu();
		GetTree().Paused = false;
		PlayerController.Instance.ChangeMouseCapturing(true);
		if (_wasAwaitingInteraction) {
			ShowInteractableMessage();
		} else if(!_wasAwaitingInteraction && !_inNPCInteraction) {
			HideInteractableMessage();
		} else if(!_wasAwaitingInteraction && _inNPCInteraction) {
			HideInteractableMessage();
			PlayerController.Instance.ChangeMouseCapturing(false);
		}
	}
	private void MainMenuButtonPressed() {
		_wasAwaitingInteraction = false;
		_inMainMenu = true;
		HidePauseMenu();
		GetTree().Paused = false;
		HideInteractableMessage();
		_NPCInteraction.Hide();
		GameManager.Instance.GoToLevel(0);
	}

	private void QuitButtonPressed() {
		GetTree().Quit();
	}
}