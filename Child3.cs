using Godot;
using System;

public partial class Child3 : CharacterBody2D {
	[Export]
	PackedScene candy;

	[Export]
	float r =
		25;

	[Export]
	float speed =
		330;

	[Export]
	float variation =
		100;

	[Export]
	AnimatedSprite2D[] sprites;

	bool scareable = false;
	bool seen = false;
	bool inRange = false;
	bool scared = false;
	Vector2 localSpeed = new Vector2(0, 0);
	Vector2 globalSpeed = new Vector2(0, 0);
	float rotSpeed = 0;
	float stopCounter = 0;
	float startCounter = 0;
	float seenCounter = 0;
	float spinCounter = 0;

	private AnimatedSprite2D sprite;

	public override void _Ready() {
		foreach (AnimatedSprite2D s in sprites) {
			s.Visible = false;
		}

		sprite = sprites[(int)GD.RandRange(0, sprites.Length - 1)];
		sprite.Visible = true;
		Rotation = GD.Randf() * (float)Math.PI;
	}


// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (seen) {
			seenCounter += (float)delta;
		} else {
			seenCounter = 0;
		}

		if (seenCounter > 0.5) {
			GetTree().ReloadCurrentScene();
		}

		if (!scared) {
			scareable = !seen && inRange;

			stopCounter += GD.Randf();
			spinCounter += GD.Randf();

			localSpeed.Y += GD.Randf() * variation * (float)delta;
			localSpeed.Y -= GD.Randf() * variation * (float)delta * 0.1f;
			localSpeed.X += GD.Randf() * variation * (float)delta * 0.1f;
			localSpeed.X -= GD.Randf() * variation * (float)delta * 0.1f;

			rotSpeed += (GD.Randf() - 0.5f) * (float)delta;
			rotSpeed = clamp(rotSpeed, -0.02f, 0.02f);
			localSpeed = clamp(localSpeed.Length(), -speed, speed) * localSpeed.Normalized();
			globalSpeed = localSpeed.Rotated(Rotation);
			if (spinCounter <= 70) {
				Rotation += rotSpeed;
			} else {
				Rotation += rotSpeed * 3;
				spinCounter -= GD.Randf() * 2;
			}

			if (stopCounter <= 50) {
				Velocity = globalSpeed;
				startCounter = 0;
			} else {
				Velocity = Vector2.Zero;
				startCounter += GD.Randf();
			}

			if (startCounter >= 20) {
				stopCounter = 0;
			}

			MoveAndSlide();
		} else {
			Position += new Vector2(0, 5);
			// $CollisionShape2D.set_deferred("disabled", true);
			// $echildView / VisionCone2D.visible = false;
			if (Position.Y > 50000) {
				QueueFree();
			}
		}
		// $Node2D / Label.visible = scareable && !scared;
		// $Node2D.global_Rotation = 0;

		if (Velocity.Length() > 0) {
			sprite.Play();
		}

		if (Velocity.Y > 0 && Velocity.X > 0) {
			if (Velocity.Y > Velocity.X) {
				sprite.Play("down");
			}

			if (Velocity.Y < Velocity.X) {
				sprite.Play("right");
			}
		} else if (Velocity.Y > 0 && Velocity.X < 0) {
			if (Velocity.Y > Math.Abs(Velocity.X)) {
				sprite.Play("down");
			}

			if (Velocity.Y < Math.Abs(Velocity.X)) {
				sprite.Play("left");
			}
		} else if (Velocity.Y < 0 && Velocity.X > 0) {
			if (Math.Abs(Velocity.Y) < Math.Abs(Velocity.X)) {
				sprite.Play("left");
			}

			if (Math.Abs(Velocity.Y) > Math.Abs(Velocity.X)) {
				sprite.Play("up");
			}
		} else if (Velocity.Y < 0 && Velocity.X < 0) {
			if (Math.Abs(Velocity.Y) < Math.Abs(Velocity.X)) {
				sprite.Play("right");
			}

			if (Math.Abs(Velocity.Y) > Math.Abs(Velocity.X)) {
				sprite.Play("up");
			}
		} else {
			if (Velocity.Y > 0) {
				sprite.Play("down");
			}

			if (Velocity.Y < 0) {
				sprite.Play("up");
			}

			if (Velocity.X > 0) {
				sprite.Play("right");
			}

			if (Velocity.X < 0) {
				sprite.Play("left");
			} else {
				sprite.Stop();
			}
		}
	}

	public void scare() {
		if (scareable && !scared) {
			for (int i = 0; i < 5; i++) {
				Area2D a = (Area2D)candy.Instantiate();
				a.Position = Position + new Vector2((int)GD.RandRange(-r, r), (int)GD.RandRange(-r, r));
				GetParent().AddChild(a);
				scared = true;
			}
		}
	}

	public void _on_area_2d_area_entered(Area2D area) {
		seen = true;
	}

	public void _on_area_2d_area_exited(Area2D area) {
		seen = false;
	}

	public void _on_area_entered(Area2D area) {
		inRange = true;
	}

	public void _on_area_exited(Area2D area) {
		inRange = false;
	}

	private float clamp(float i, float min, float max) {
		return Math.Max(Math.Min(i, max), min);
	}
}
