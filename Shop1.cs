using Godot;
using System;
using System.Collections.Generic;

public partial class Shop1 : CanvasLayer {
    [Export]
    private PackedScene upgradeWidget;
    [Export]
    private CharacterBody2D player;
    
    List<Upgrade> upgrades = new List<Upgrade>();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        upgrades.Add(new ScareUpgrade((Player2)player, (Map)GetParent()));
        upgrades.Add(new HardUpgrade((Player2)player));
        // TODO: add new upgrades to shop here
        
        
        foreach (Upgrade u in upgrades) {
            var w = upgradeWidget.Instantiate();
            u.init();
            ((UpgradeWidget)w).setUpgrade(u);
            ((UpgradeWidget)w).setShop(this);
            GetNode("items/MarginContainer/ScrollContainer/VBoxContainer").AddChild(w);
            
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public void setSelection(UpgradeData d) {
        GetNode<RichTextLabel>("back/title").Text = d.name;
        GetNode<RichTextLabel>("back/how").Text = d.description;
        GetNode<RichTextLabel>("textbox/desc").Text = d.hovertext;
        
    }

    public bool charge(int cost) {
        if ((int)player.Get("score") >= cost) {
            player.Set("score",  (int)player.Get("score") - cost);
            return true;
        }
        return false;
    }
    public bool canCharge(int cost) {
        return (int)player.Get("score") >= cost;
    }

    public void refund(int cost) {
        player.Set("score", (int)player.Get("score") + cost);
    }
}