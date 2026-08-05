using Godot;
using System;

public partial class GameManager : Node3D {
	public static GameManager Instance { get; private set; }

	[Signal]
	public delegate void InLobbyEventHandler(bool inLobby);
	[Export] private string[] _levelListUids;
	private static int _levelNumber = 0;
	public bool IsInLobby { get; private set; } = true;

    public override void _Ready() {
        Instance = this;
		LevelManager.Instance.LoadLevelAsync(_levelListUids[0]);
    }

	public void GoToLevel(int value) {
		_levelNumber = value;
		if (value == 0) {
			IsInLobby = true;
		} else {
			IsInLobby = false;
		}
		EmitSignal(SignalName.InLobby, IsInLobby);
		LevelManager.Instance.LoadLevelAsync(_levelListUids[_levelNumber]);
	}
}