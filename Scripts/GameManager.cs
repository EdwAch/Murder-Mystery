using Godot;
using System;

public partial class GameManager : Node3D {
	public static GameManager Instance { get; private set; }

	[Signal]
	public delegate void InLobbyEventHandler(bool inLobby);
	[Export] private PackedScene[] _levelList;
	private static int _levelNumber = 0;
	public bool IsInLobby { get; private set; } = true;

    public override void _Ready() {
        Instance = this;
		LevelManager.Instance.LoadLevel(_levelList[0]);
    }

	public void GoToLevel(int value) {
		_levelNumber = value;
		IsInLobby = _levelNumber == 0;
		EmitSignal(SignalName.InLobby, IsInLobby);
		LevelManager.Instance.LoadLevel(_levelList[_levelNumber]);
	}
}