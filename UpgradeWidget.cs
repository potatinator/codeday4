using Godot;
using System;


public partial class UpgradeWidget : Control {
    [Export]
    public Texture2D disabledImage;
    private Upgrade     u;

    private Shop1 s = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        u.update((float)delta);
        
        if (u.data.level > 0) {
            if (u.data.maxLevel > 0) {
                GetNode<RichTextLabel>("lv").Text = "Level " + u.data.level + "/" + u.data.maxLevel + ", Cost: " + u.getCost() + " Candy";
            } else {
                GetNode<RichTextLabel>("lv").Text = "Level " + u.data.level + ", Cost: " + u.getCost() + " Candy";
            }
            GetNode<RichTextLabel>("lv").Visible      = true;
            GetNode<Button>("lv down").Visible = true;
        } else {
            GetNode<RichTextLabel>("lv").Visible      = false;
            GetNode<Button>("lv down").Visible = false;
        }
        if (!s.canCharge(u.data.cost) || (u.data.level >= u.data.maxLevel  && u.data.maxLevel > 0)) {
            GetNode<Button>("lv up").Disabled = true;
            GetNode<Button>("lv up").Icon = disabledImage;
        } else {
            GetNode<Button>("lv up").Disabled = false;
            GetNode<Button>("lv up").Icon = null;
        }
        
        GetNode<RichTextLabel>("RichTextLabel").Text = u.data.name;
        GetNode<TextureRect>("TextureRect").Texture  = u.data.icon;
        TooltipText                                  = u.data.hovertext;

    }

    public void _on_gui_input(InputEvent e) {
        if (e is InputEventMouseButton) {
            if (((InputEventMouseButton)e).ButtonIndex.Equals(MouseButton.Left) && ((InputEventMouseButton)e).Pressed) {
                s.setSelection(u.data);
            }
        }
    }

    public void setUpgrade(Upgrade u) {
        this.u = u;

    }

    public void setShop(Shop1 s) {
        this.s = s;
    }

    public void _on_lv_up_pressed() {
        if (u.data.level < u.data.maxLevel || u.data.maxLevel <= 0) {
            if (s.charge(u.getCost())) {
                u.upgrade();
            }
            u.update(0f);
            s.setSelection(u.data);
        }
    }
    public void _on_lv_down_pressed() {
        if (u.data.level > 0) {
            u.data.level -= 1;
            s.refund(u.getCost());
            u.update(0f);
            s.setSelection(u.data);
        }
    }
}