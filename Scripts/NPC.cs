using Godot;
using System;

public partial class NPC : Node3D {
	
	public void StartInteraction() {
		NPCInteraction.Instance.ShowNPCInteraction();
	}
}