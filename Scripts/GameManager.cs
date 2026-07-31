using Godot;
using System;

public partial class GameManager : Node3D {
	public static GameManager Instance { get; private set; }

	[Export] private PackedScene[] _levelList;
	private static int _levelNumber = 0;

    public override void _Ready() {
        Instance = this;
		LevelManager.Instance.LoadLevel(_levelList[0]);
    }
}