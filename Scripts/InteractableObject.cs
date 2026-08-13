using Godot;
using System;

public partial class InteractableObject : StaticBody3D {

	private enum InteractType { Door, BreakableObject, NPC}
	[Export] private InteractType Type;
	[Export] private Node3D _objectToBeInteracted;
    public override void _Ready() {
        CallDeferred(MethodName.SubscribeToSignals);
	}
	public void Interact(Node3D interactor) {
		switch (Type) {
			case InteractType.Door:
				if (_objectToBeInteracted is Door door) {
					door.ChangeDoorState();
				}
				break;
			case InteractType.BreakableObject:
				_objectToBeInteracted.QueueFree();
				break;
			case InteractType.NPC:
				if (_objectToBeInteracted is NPC npc) {
					npc.StartInteraction();
				}
				break;	
		}
	}

	public void ShowLabel(bool value) {
		if (value) {
			UI.Instance.ShowInteractableMessage();
		} else {
			UI.Instance.HideInteractableMessage();
		}
	}

	private void OnGamePaused(bool isPaused) {
		ShowLabel(!isPaused);
	}

	private void SubscribeToSignals() {
		UI.Instance.GamePaused += OnGamePaused;
	}
}