using Godot;
using System;

public partial class LevelManager : Node {
	public static LevelManager Instance { get; private set; }

	private Node3D _container;
	private CharacterBody3D _player;
    public override void _Ready() {
        Instance = this;
    }

	public void RegisterPlayer(CharacterBody3D player) => _player = player;
	public void RegisterContainer(Node3D container) => _container = container;

	public void LoadLevel(PackedScene levelPath) {
		if (_player == null || _container == null) {
			return;
		}
		foreach (Node child in _container.GetChildren()) {
			child.QueueFree();
		}
		
		var newLevel = levelPath.Instantiate<Node3D>();
		_container.AddChild(newLevel);

		var spawn = newLevel.GetNode<Marker3D>("SpawnPoint");
		if (spawn != null) {
			_player.GlobalPosition = spawn.GlobalPosition;
		}
	}
}