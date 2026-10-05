using Godot;
using System;

public partial class NPCInteraction : MarginContainer {
	public static NPCInteraction Instance { get; private set; }
	[Signal]
	public delegate void InNPCInteractionEventHandler(bool inInteraction);
	private bool _inInteraction = false;
	private bool _wasInteracting = false;

    public override void _Ready() {
		Instance = this;
		HideNPCInteraction();
        CallDeferred(MethodName.SubscribeToSignals);
    }

	private void SubscribeToSignals() {
		UI.Instance.GamePaused += OnGamePaused;
		GameManager.Instance.InLobby += IsInLobby;
	}

	public void ShowNPCInteraction() {
		this.Show();
		_inInteraction = true;
		EmitSignal(SignalName.InNPCInteraction, true);
	}

	public void HideNPCInteraction() {
		this.Hide();
		_inInteraction = false;
		EmitSignal(SignalName.InNPCInteraction, false);
	}

	private void OnGamePaused(bool isPaused) {
		if (isPaused && _inInteraction) {
			HideNPCInteraction();
			_wasInteracting = true;
		} else if (!isPaused && _wasInteracting) {
			ShowNPCInteraction();
			_wasInteracting = false;
		}
	}

	private void IsInLobby(bool inLobby) {
		if (inLobby) {
			HideNPCInteraction();
			_wasInteracting = false;
			EmitSignal(SignalName.InNPCInteraction, false);
		}
	}
}