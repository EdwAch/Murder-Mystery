using Godot;
using System;

public partial class PlayerController : CharacterBody3D {
	[Export] private float _speed;
	[Export] private float _jumpVelocity;
	[Export] private float _gravity;
	[Export] private float _mouseSensitivity;
	[Export] private Camera3D _camera;
	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor()) {
			velocity.Y -= _gravity * (float)delta;
		}

		if (Input.IsActionJustPressed("Jump") && IsOnFloor()) {
			velocity.Y += _jumpVelocity;
		}
		
		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * _speed;
			velocity.Z = direction.Z * _speed;
		} else {
			velocity.X = Mathf.Lerp(velocity.X, 0, 10f * (float)delta);
			velocity.Z = Mathf.Lerp(velocity.Z, 0, 10f * (float)delta);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
    
	public override void _UnhandledInput(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseMotion) {
			RotateY(-mouseMotion.Relative.X * _mouseSensitivity);

			Vector3 cameraRotation = _camera.Rotation;
			cameraRotation.X = Mathf.Clamp(cameraRotation.X - mouseMotion.Relative.Y * _mouseSensitivity, Mathf.DegToRad(-89f), Mathf.DegToRad(89f));
			_camera.Rotation = cameraRotation;
		}
    }
}