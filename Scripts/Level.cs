using Godot;
using System;

public partial class Level : Node3D {

    public override void _Ready() {
        CallDeferred(MethodName.SubscribeToSignals);
    }

	private void SubscribeToSignals() {
		GameManager.Instance.InLobby += PlayerInLobby;
		PlayerInLobby(GameManager.Instance.IsInLobby);
	}
	private void PlayerInLobby(bool inLobby) {
		if (inLobby) {
			PlayerController.Instance.DetachCamera(true);
			PlayerController.Instance.DisableMovementInput(true);
		} else {
			PlayerController.Instance.DetachCamera(false);
			PlayerController.Instance.DisableMovementInput(false);
		}
	}
}