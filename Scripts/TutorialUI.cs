using Godot;
using System;

public partial class TutorialUI : MarginContainer {
	
	[Export] private RichTextLabel _tutorialText;
	[Export] private string[] _tutorials;
	[Export] private ColorRect _loadingBar;
	private Tween _tween;
	private Tween _loadingTween;
	private int _tutorialNumber = 0;
	private bool _hasMoved = false;
	private bool _hasJumped = false;
	private bool _inLobby;
	private bool _inLevelOne = false;
	private const float FadeTime = 0.3f;
	public override void _Ready() {
		CallDeferred(MethodName.SubscribeToUI);
		UpdateTutorialText(_tutorials[_tutorialNumber]);
		_tutorialNumber++;
	}

    public override void _PhysicsProcess(double delta) {
		if (!_inLobby) {
			Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");

        	if (!_hasMoved && inputDir != Vector2.Zero) {
				_hasMoved = true;
				UpdateTutorialText(_tutorials[_tutorialNumber], 2);
				_tutorialNumber++;
			}

			if (_hasMoved && !_hasJumped && Input.IsActionJustPressed("Jump")) {
				_hasJumped = true;
				UpdateTutorialText(_tutorials[0], 2);
				_tutorialNumber = 0;
			}
		}
    }

	private void SubscribeToUI() {
		UI.Instance.GamePaused += OnGamePaused;
		//Level.Instance.InLobby += InLobby;
	}

	private void OnGamePaused(bool isPaused) {
		if (isPaused) {
			_tween?.Pause();
			_loadingTween?.Pause();
			HideTutorialHUD();
		} else {
			_tween?.Play();
			_loadingTween?.Play();
			ShowTutorialHUD();
		}
	}

	/*private void InLobby(bool inLobby) {
		_inLobby = inLobby;
		if (_inLobby) {
			this.Visible = false;
		} else if (_inLevelOne) {
			this.Visible = true;
		}
	}*/

	private void UpdateTutorialText(string str, float waitTime = 0f) {
		_tween?.Kill();
		_tween = CreateTween();
		if (waitTime != 0f) {
			_tween.TweenInterval(waitTime);
			LoadingBar(waitTime);
		}
		if (_tutorialText.Text != "") {
			_tween.TweenProperty(_tutorialText, "modulate:a", 0, FadeTime);
			_tween.TweenInterval(1);
		}
		_tween.TweenCallback(Callable.From(() => _tutorialText.Text = str));
		_tween.TweenProperty(_tutorialText, "modulate:a", 1, FadeTime);
	}

	private void LoadingBar(float time) {
		_loadingTween?.Kill();
		_loadingTween = CreateTween();
		_loadingTween.TweenProperty(_loadingBar, "anchor_right", 1, time);
		_loadingTween.TweenProperty(_loadingBar, "modulate:a", 0, FadeTime);
		_loadingTween.TweenInterval(FadeTime);
		_loadingTween.TweenProperty(_loadingBar, "anchor_right", 0, 0);
		_loadingTween.TweenProperty(_loadingBar, "modulate:a", 1, FadeTime);
	}

	private void HideTutorialHUD() {
		this.Hide();
	}

	private void ShowTutorialHUD() {
		this.Show();
	}
}