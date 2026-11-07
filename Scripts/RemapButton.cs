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

	private static bool IsReservedKey(Key key) => key == Key.Meta || key == Key.Hyper || key == Key.Menu || key == Key.None || key == Key.Unknown || (key >= Key.F1 && key <= Key.F35);

	private bool IsKeyAlreadyInUse(Key key) {
		foreach (StringName action in InputMap.GetActions()) {
			string actionName = action.ToString();
			if (actionName == _name) continue;
			if (actionName.StartsWith("ui_")) continue;

			foreach (InputEvent ev in InputMap.ActionGetEvents(action)) {
				if (ev is InputEventKey k && k.PhysicalKeycode == key) return true;
			}
		}
		return false;
	}

    public override void _UnhandledKeyInput(InputEvent @event) {
        if (!_listeningForInput) return;

		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.IsEcho()) {
			Key KEY = keyEvent.PhysicalKeycode;

			if (IsReservedKey(KEY)) {
				this.Text = "Key not allowed";
				return;
			}
			if (IsKeyAlreadyInUse(KEY)) {
				this.Text = "Key already in use";
				return;
			}
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