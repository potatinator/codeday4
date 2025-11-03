using Godot;
using System;

public partial class Child3 : CharacterBody2D {
	[Export]
	PackedScene candy;


	[Export]
	float speed =
		330;

	[Export]
	float variation =
		100;

	[Export]
	AnimatedSprite2D[] sprites;

	[Export]
	private float scareTime = 0.1f;

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
	float scareCounter = 0;
	private float r;

	private AnimatedSprite2D sprite;

	public override void _Ready() {
		foreach (AnimatedSprite2D s in sprites) {
			s.Visible = false;
		}

		((Sprite2D)GetNode("Node2D/Sprite2D")).Scale = ((Sprite2D)GetNode("Node2D/Sprite2D")).Scale with { X = 0 };

		sprite = sprites[(int)GD.RandRange(0, sprites.Length - 1)];
		sprite.Visible = true;
		((Area2D)GetNode("echildView")).Rotation = GD.Randf() * (float)Math.PI;

		float a = 3f + (scareTime * 1f);
		sprite.Scale = new Vector2(a, a);
		((Area2D)GetNode("Node2D/Area2D")).Scale = new Vector2(a / 3, a / 3);
		((Area2D)GetNode("Node2D/echildView")).Scale = new Vector2(a / 3, a / 3);

		r = 25 + (scareTime * 13);
	}


// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		scareCounter -= 0.5f * (float)delta;
		scareCounter = clamp(scareCounter, 0, scareTime);
		((Sprite2D)GetNode("Node2D/Sprite2D")).Scale =
			((Sprite2D)GetNode("Node2D/Sprite2D")).Scale with { X = scareCounter * 150 / scareTime };


		if (scareCounter >= scareTime) {
			scare();
		}

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
			globalSpeed = localSpeed.Rotated(((Area2D)GetNode("echildView")).Rotation);
			if (spinCounter <= 70) {
				((Area2D)GetNode("echildView")).Rotation += rotSpeed;
			} else {
				((Area2D)GetNode("echildView")).Rotation += rotSpeed * 3;
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
			((CollisionShape2D)GetNode("CollisionShape2D")).SetDeferred("disabled", true);
			((CollisionPolygon2D)GetNode("echildView/CollisionPolygon2D")).SetDeferred("disabled", true);
			((Area2D)GetNode("echildView")).Visible = false;
			if (Position.Y > 50000) {
				QueueFree();
			}
		}
		((Label)GetNode("Node2D/Label")).Visible = scareable && !scared;
		((Sprite2D)GetNode("Node2D/Sprite2D")).Visible = scareable && !scared;
		((Node2D)GetNode("Node2D")).GlobalRotation = 0;

		if (Velocity.Length() > 0) {
			sprite.Play();
		}

		#region "anims"

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

		#endregion
	}

	public void incrementScare() {
		scareCounter += (float)GetProcessDeltaTime() * 1.5f; //times 1.5 to account for 0.5x fade 
	}

	public void scare() {
		if (scareable && !scared) {
			for (int i = 0; i < (5 + (int)(scareTime * 10)); i++) {
				Area2D a = (Area2D)candy.Instantiate();
				a.Position = Position + new Vector2((int)GD.RandRange(-r, r), 0).Rotated(GD.Randf()*2f*(float)Math.PI);
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
