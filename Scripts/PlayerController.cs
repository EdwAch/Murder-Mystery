using Godot;
using System;

public partial class PlayerController : CharacterBody3D {
	public static PlayerController Instance { get; private set; }
	[Export] private float _speed;
	[Export] private float _jumpVelocity;
	[Export] private float _gravity;
	[Export] private float _mouseSensitivity;
	[Export] private Camera3D _camera;
	private bool _pauseMenuShown = false;
	private bool _inMainMenu = true;
	private bool _disableMovementInput = false;
	public override void _Ready() {
		LevelManager.Instance.RegisterPlayer(this);
		Instance = this;
		//ChangeMouseCapturing();
	}

    public override void _Process(double delta) {
        if (!_inMainMenu && Input.IsActionJustPressed("Escape")) {
			if (_pauseMenuShown) {
				UI.Instance.HidePauseMenu(); 
			} else {
				UI.Instance.ShowPauseMenu();
			}
			_pauseMenuShown = !_pauseMenuShown;
			ChangeMouseCapturing();
		}
    }

	public override void _PhysicsProcess(double delta)	{
		Vector3 velocity = Velocity;

		if (!_disableMovementInput) {
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
	}
    
	public override void _UnhandledInput(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured) {
			RotateY(-mouseMotion.Relative.X * _mouseSensitivity);

			Vector3 cameraRotation = _camera.Rotation;
			cameraRotation.X = Mathf.Clamp(cameraRotation.X - mouseMotion.Relative.Y * _mouseSensitivity, Mathf.DegToRad(-89f), Mathf.DegToRad(89f));
			_camera.Rotation = cameraRotation;
		}
    }

	public void ChangeMouseCapturing() {
		if (Input.MouseMode == Input.MouseModeEnum.Captured) {
			Input.MouseMode = Input.MouseModeEnum.Visible;
		} else {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public void ChangePauseMenuShown(bool value) {
		_pauseMenuShown = value;
	}

	public void DetachCamera(bool value) {
		if (value) {
			_camera.Current = false;
		} else {
			_camera.Current = true;
		}
	}

	public void DisableMovementInput(bool value) {
		_disableMovementInput = value;
	}

	public void ChangeInMainMenuBool(bool value) {
		_inMainMenu = value;
	}
}