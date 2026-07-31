using Godot;
using System;

public partial class Level : Node3D {
	public static Level Instance { get; private set; }
	[Export] private bool _isLobby;
	[Signal]
	public delegate void InLobbyEventHandler(bool inLobby);

    public override void _Ready() {
        if (_isLobby) {
			EmitSignal(SignalName.InLobby, true);
			PlayerController.Instance.DetachCamera(true);
			PlayerController.Instance.DisableMovementInput(true);
		} else {
			EmitSignal(SignalName.InLobby, false);
		}
    }
}