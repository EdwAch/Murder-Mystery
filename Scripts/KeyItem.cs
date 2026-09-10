using Godot;
using System;

public partial class KeyItem : Node3D {
	
	[Export] public KeyItemChecklist.KeyItems _keyItem;
	public void Interact() {
		KeyItemChecklist.Instance.AddKeyItem(_keyItem);
	}
}