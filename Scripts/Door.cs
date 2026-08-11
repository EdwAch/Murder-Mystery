using Godot;
using System;

public partial class Door : Node3D {
	
	[Export] private AnimatableBody3D _door;
	private Tween _tween;
	private bool _isOpen = false;
	public void ChangeDoorState() {
		_tween?.Kill();
		_tween = CreateTween();
		if (_isOpen) {
			_tween.TweenProperty(_door, "rotation:y", Mathf.DegToRad(0), 0.5).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
			_isOpen = false;
		} else {
			_tween.TweenProperty(_door, "rotation:y", Mathf.DegToRad(-90), 0.5).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
			_isOpen = true;
		}
	}
}