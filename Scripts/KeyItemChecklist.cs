using Godot;
using System;

public partial class KeyItemChecklist : Node {
	public static KeyItemChecklist Instance { get; private set; }
	public override void _Ready() {
		Instance = this;
	}
}