using Godot;
using System;

public partial class RemapButton : Button {
	
	[Export] private string _name;
	private bool _listeningForInput = false;

    public override void _Ready() {
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

			this.Text = physicalEvent.AsTextPhysicalKeycode();
			if (_name == "Interact") {
				InteractablePromptKeyGetting.Instance.NewKeyUsed(physicalEvent.AsTextPhysicalKeycode());
			}
			_listeningForInput = false;
			GetViewport().SetInputAsHandled();
			SettingsMenu.Instance.AwaitingKeyInput(false);
			SettingsMenu.Instance.SettingsChanged();
		}
    }
}