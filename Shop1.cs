using Godot;
using System;
using System.Collections.Generic;

public partial class Shop1 : CanvasLayer {
    List<upgrade> upgrades = new List<upgrade>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        upgrades.Add(new TestUpgrade());
        // TODO: add new upgrades to shop here
        foreach (upgrade u in upgrades) {
            
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }
}