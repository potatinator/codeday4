using Godot;
using System;

public partial class egg : Area2D
{
    public override void _Ready() {
    }

    public void _on_area_2d_area_entered(Area2D area) {
        GetTree().ReloadCurrentScene();
    }
}
