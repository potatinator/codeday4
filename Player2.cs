using Godot;
using System;

public partial class Player2 : CharacterBody2D {
    [Export]
    float speedBase = 200;
    [Export]
    float speedSprint = 400;
    [Export]
    float speedSneak = 100;
    [Export]
    CanvasLayer hud;
    GodotObject  HUD;
    float        speed      = 200;
    Vector2      vel        = Vector2.Zero;
    int          score      = 0;
    bool         scareReady = false;
    GodotObject  toScare;
    float        stamina   = 1;
    bool         bigChild  = false;
    public bool  canSprint = true;
    public bool  paused    = false;
    public float scareRate = 1f;
    public float candyMult = 1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        HUD = hud;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        vel = Vector2.Zero;
        if (Input.IsActionPressed("move_down")) {
            vel.Y += 1;
        }
        if (Input.IsActionPressed("move_up")) {
            vel.Y -= 1;
        }
        if (Input.IsActionPressed("move_left")) {
            vel.X -= 1;
        }
        if (Input.IsActionPressed("move_right")) {
            vel.X += 1;
        }
        if (Input.IsActionPressed("sneak")) {
            speed = speedSneak;
        } else {
            if (Input.IsActionPressed("sprint")) {
                if (canSprint) {
                    speed = speedSprint;
                    if (stamina <= 0) {
                        canSprint = false;
                    }
                } else {
                    speed = speedBase;
                }
            } else {
                speed     = speedBase;
                canSprint = stamina > 0.05;
            }
        }

        if (vel.Length() > 0 && !paused) {
            vel = vel.Normalized() * speed;
            if (vel.Y > 0 && vel.X > 0) {
                if (vel.Y > vel.X) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("down");
                }
                if (vel.Y < vel.X) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("right");
                }
            } else if (vel.Y > 0 && vel.X < 0) {
                if (vel.Y > Math.Abs(vel.X)) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("down");
                }
                if (vel.Y < Math.Abs(vel.X)) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("left");
                }
            } else if (vel.Y < 0 && vel.X > 0) {
                if (Math.Abs(vel.Y) < Math.Abs(vel.X)) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("left");
                }
                if (Math.Abs(vel.Y) > Math.Abs(vel.X)) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("up");
                }
            } else if (vel.Y < 0 && vel.X < 0) {
                if (Math.Abs(vel.Y) < Math.Abs(vel.X)) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("right");
                }
                if (Math.Abs(vel.Y) > Math.Abs(vel.X)) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("up");
                }
            } else {
                if (vel.Y > 0) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("down");
                }
                if (vel.Y < 0) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("up");
                }
                if (vel.X > 0) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("right");
                }
                if (vel.X < 0) {
                    ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("left");
                }
            }
        } else {
            ((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Stop();
        }
        
        stamina += 0.025f * (float)delta + (1.1f*speedBase-vel.Length()) * 0.002f * (float)delta;

        if (!paused) {
            Velocity = vel;
            MoveAndSlide();
        }

        stamina = Math.Clamp(stamina, 0, 1);

        HUD.Call("setScore", score);
        HUD.Call("setStamina", stamina);

        if (Input.IsActionPressed("scare")) {
            if (scareReady) {
                if (toScare != null) {
                    toScare.Call("incrementScare", scareRate);
                }
            }
        }
    }

    public void _on_pickup_area_entered(Area2D area) {
        area.QueueFree();
        score   += 1 * (int)candyMult;
        stamina += 0.05f * candyMult;
    }

    public void _on_scare_area_entered(Area2D area) {
        if (area.GetParent().Name.ToString().Contains("child")) {
            scareReady = true;
            bigChild   = false;
            toScare    = area.GetParent();
        }
    }

    public void _on_scare_area_exited(Area2D area) {
        if (area.GetParent().Name.ToString().Contains("child")) {
            scareReady = false;
            toScare    = null;
        }
    }
}