using Godot;
using System;
using System.Collections.Generic;

public partial class KeyItemChecklist : Node {
	public static KeyItemChecklist Instance { get; private set; }
	public enum KeyItems { temp}
	private HashSet<KeyItems> _keyItems = new();
	public override void _Ready() {
		Instance = this;
	}

	public bool HasKeyItems(KeyItems keyItem) {
		return _keyItems.Contains(keyItem);
	}

	public void AddKeyItem(KeyItems keyItem) {
		_keyItems.Add(keyItem);
	}

	public void RemoveKeyItem(KeyItems keyItem) {
		_keyItems.Remove(keyItem);
	}
}