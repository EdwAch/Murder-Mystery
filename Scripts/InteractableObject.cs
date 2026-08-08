using Godot;
using System;

public partial class InteractableObject : StaticBody3D {

    public override void _Ready() {
        CallDeferred(MethodName.SubscribeToSignals);
	}
	public void Interact(Node3D interactor) {
		return;
	}

	public void ShowLabel(bool value) {
		if (value) {
			UI.Instance.ShowInteractableMessage();
		} else {
			UI.Instance.HideInteractableMessage();
		}
	}

	private void OnGamePaused(bool isPaused) {
		ShowLabel(!isPaused);
	}

	private void SubscribeToSignals() {
		UI.Instance.GamePaused += OnGamePaused;
	}
}