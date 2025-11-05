using Godot;
using System;

public partial class Map : Node2D {
    private bool paused = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("pause")) {
            if (paused) {
                play();
            } else {
                pause();
            }
        }
    }

    public void pause() {
        foreach (Node n in GetNode("kids").GetChildren()) {
            if (n.Name.ToString().Contains("child")) {
                n.Call("pause");
            }
        }
        GetNode("player2").Set("paused", true);
        paused = true;
    }

    public void play() {
        paused = false;
        foreach (Node n in GetNode("kids").GetChildren()) {
            if (n.Name.ToString().Contains("child")) {
                n.Call("play");
            }
        }
        GetNode("player2").Set("paused", false);
    }

    public void _on_area_2d_area_entered(Area2D area) {
        GetNode<Label>("Label").Visible = true;
    }
    public void _on_area_2d_area_exited(Area2D area) {
        GetNode<Label>("Label").Visible = false;
    }
}