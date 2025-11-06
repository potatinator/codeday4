using Godot;
using System;

public partial class Map : Node2D {
    private bool paused  = false;
    private bool canShop = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("shop")) {
            if (paused) {
                exitShop();
            } else {
                if (canShop) {
                    enterShop();
                }
            }
        }
    }

    public void enterShop() {
        foreach (Node n in GetNode("kids").GetChildren()) {
            if (n.Name.ToString().Contains("child")) {
                n.Call("pause");
            }
        }
        GetNode("player2").Set("paused", true);
        paused                                = true;
        GetNode<CanvasLayer>("shop1").Visible = true;
        GetNode<CanvasLayer>("HUD").GetNode<Control>("Control").Visible = false;
    }

    public void exitShop() {
        paused = false;
        foreach (Node n in GetNode("kids").GetChildren()) {
            if (n.Name.ToString().Contains("child")) {
                n.Call("play");
            }
        }
        GetNode("player2").Set("paused", false);
        GetNode<CanvasLayer>("shop1").Visible                           = false;
        GetNode<CanvasLayer>("HUD").GetNode<Control>("Control").Visible = true;
    }

    public void _on_area_2d_area_entered(Area2D area) {
        GetNode<Label>("Label").Visible = true;
        canShop                         = true;
    }
    public void _on_area_2d_area_exited(Area2D area) {
        GetNode<Label>("Label").Visible = false;
        canShop                         = false;
    }
}