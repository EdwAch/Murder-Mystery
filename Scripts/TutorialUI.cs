using Godot;
using System;

public partial class TutorialUI : MarginContainer {
	
	[Export] private RichTextLabel _tutorialText;
	[Export] private string[] _tutorials;
	private Tween _tween;
	private int _tutorialNumber = 0;
	private const float FadeTime = 0.3f;
	public override void _Ready() {
		CallDeferred(MethodName.SubscribeToUI);
		UpdateTutorialText(_tutorials[_tutorialNumber]);
		_tutorialNumber++;
	}

	private void SubscribeToUI() {
		UI.Instance.GamePaused += OnGamePaused;
	}

	private void OnGamePaused(bool isPaused) {
		if (isPaused) {
			_tween?.Pause();
			HideTutorialHUD();
		} else {
			_tween?.Play();
			ShowTutorialHUD();
		}
	}

	private void UpdateTutorialText(string str) {
		_tween?.Kill();
		_tween = CreateTween();
		if (_tutorialText.Text != "") {
			_tween.TweenProperty(_tutorialText, "modulate:a", 0, FadeTime);
			_tween.TweenInterval(1);
		}
		_tween.TweenCallback(Callable.From(() => _tutorialText.Text = str));
		_tween.TweenProperty(_tutorialText, "modulate:a", 1, FadeTime);
	}

	private void HideTutorialHUD() {
		this.Hide();
	}

	private void ShowTutorialHUD() {
		this.Show();
	}
}