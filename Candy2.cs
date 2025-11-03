using Godot;
using System;

public partial class Candy2 : Area2D {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Color col = HSVToRGB(GD.Randf(), 1f, 1f).Lightened(5);
        ((ColorRect)GetNode("ColorRect")).Color = col;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    private Color HSVToRGB(float h, float s, float v, float a = 1f) {
        float r = 0;
        float g = 0;
        float b = 0;

        float i = (float)Math.Floor(h * 6);
        float f = h * 6 - i;
        float p = v * (1 - s);
        float q = v * (1 - f * s);
        float t = v * (1 - (1 - f) * s);

        switch ((int)i % 6) {
            case 0:
                r = v;
                g = t;
                b = p;
            break;
            case 1:
                r = q;
                g = v;
                b = p;
            break;
            case 2:
                r = p;
                g = v;
                b = t;
            break;
            case 3:
                r = p;
                g = q;
                b = v;
            break;
            case 4:
                r = t;
                g = p;
                b = v;
            break;
            case 5:
                r = v;
                g = p;
                b = q;
            break;
        }
        return new Color(r, g, b, a);
    }
}