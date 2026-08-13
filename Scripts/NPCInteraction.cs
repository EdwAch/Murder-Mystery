using Godot;
using System;

public partial class NPCInteraction : MarginContainer {
	public static NPCInteraction Instance { get; private set; }
	private bool _inInteraction = false;
	private bool _wasInteracting = false;

    public override void _Ready() {
		Instance = this;
		HideNPCInteraction();
        CallDeferred(MethodName.SubscribeToSignals);
    }

	private void SubscribeToSignals() {
		UI.Instance.GamePaused += OnGamePaused;
	}

	public void ShowNPCInteraction() {
		this.Show();
		_inInteraction = true;
	}

	public void HideNPCInteraction() {
		this.Hide();
		_inInteraction = false;
	}

	private void OnGamePaused(bool isPaused) {
		if (isPaused && _inInteraction) {
			HideNPCInteraction();
			_wasInteracting = true;
		} else if (!isPaused && _wasInteracting){
			ShowNPCInteraction();
			_wasInteracting = false;
		}
	}
}