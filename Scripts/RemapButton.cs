using Godot;
using System;

public partial class RemapButton : Button {
	public static RemapButton Instance { get; private set; }
	[Signal]
	public delegate void ButtonRemappedEventHandler(string actionName, string key);
	[Export] private string _name;
	private bool _listeningForInput = false;

    public override void _Ready() {
		Instance = this;
        Pressed += () => {
			_listeningForInput = true;
			SettingsMenu.Instance.AwaitingKeyInput(true);
			this.Text = "Press any key..";
		};
    }

    public override void _UnhandledKeyInput(InputEvent @event) {
        if (!_listeningForInput) return;

		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.IsEcho()) {
			InputMap.ActionEraseEvents(_name);

			var physicalEvent = new InputEventKey {
				PhysicalKeycode = keyEvent.PhysicalKeycode
			};
			InputMap.ActionAddEvent(_name, physicalEvent);

			string key = physicalEvent.AsTextPhysicalKeycode();
			this.Text = key;
			if (_name == "Interact") {
				InteractablePromptKeyGetting.Instance.NewKeyUsed(key);
			}
			EmitSignal(SignalName.ButtonRemapped, _name, key);
			_listeningForInput = false;
			GetViewport().SetInputAsHandled();
			SettingsMenu.Instance.AwaitingKeyInput(false);
			SettingsMenu.Instance.SettingsChanged();
		}
    }
}