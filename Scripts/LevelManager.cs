using Godot;
using System;
using System.Threading.Tasks;

public partial class LevelManager : Node {
	public static LevelManager Instance { get; private set; }

	private Node3D _container;
	private CharacterBody3D _player;
    public override void _Ready() {
        Instance = this;
    }

	public void RegisterPlayer(CharacterBody3D player) => _player = player;
	public void RegisterContainer(Node3D container) => _container = container;

	public async Task LoadLevelAsync(string uid) {
		if (_player == null || _container == null) {
			return;
		}
		long id = ResourceUid.TextToId(uid);
		string path = ResourceUid.GetIdPath(id);

		_player.SetPhysicsProcess(false);
		UI.Instance.ShowLoadingScreen();

		ResourceLoader.LoadThreadedRequest(path);
		while (ResourceLoader.LoadThreadedGetStatus(path) == ResourceLoader.ThreadLoadStatus.InProgress)
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		foreach (Node child in _container.GetChildren()) {
			child.QueueFree();
		}
		
		var scene = (PackedScene)ResourceLoader.LoadThreadedGet(path);
		var newLevel = scene.Instantiate<Node3D>();
		_container.AddChild(newLevel);

		var spawn = newLevel.GetNode<Marker3D>("SpawnPoint");
		if (spawn != null) {
			_player.GlobalPosition = spawn.GlobalPosition;
		}

		_player.SetPhysicsProcess(true);
		UI.Instance.HideLoadingScreen();
	}
}