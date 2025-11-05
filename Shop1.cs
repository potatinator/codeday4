using Godot;
using System;
using System.Collections.Generic;

public partial class Shop1 : CanvasLayer {
    [Export]
    private PackedScene upgradeWidget;
    
    List<Upgrade> upgrades = new List<Upgrade>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        upgrades.Add(new TestUpgrade());
        // TODO: add new upgrades to shop here
        foreach (Upgrade u in upgrades) {
            var w =upgradeWidget.Instantiate();
            w.Reparent(GetNode("items/VBoxContainer"));
            ((UpgradeWidget)w.GetScript()).data = u.data;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }
}