using Godot;
using System;

public partial class InteractablePromptKeyGetting : MarginContainer {
	public static InteractablePromptKeyGetting Instance { get; private set; }
	[Export] private RichTextLabel _text;
	public override void _Ready() {
		Instance = this;
	}

	public void NewKeyUsed(string key) {
		_text.Text = "[b]" + key;
	}
}