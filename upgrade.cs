using Godot;
using System;
using System.Transactions;

public class Upgrade {
    public UpgradeData data;

    public virtual void init() {
        data.level       = 0;
        data.priority    = 1;
        data.costFactor  = 0.25f;
        data.name        = "";
        data.description = "";
        data.hovertext   = "";
        data.cost        = 0;
        data.maxLevel    = -1;
        data.icon        = new PlaceholderTexture2D();
    }

    public virtual void update(float delta) {
    }

    public virtual void upgrade() {
        if (data.level < data.maxLevel || data.maxLevel <= 0) {
            data.level += 1;
        }
    }

    public virtual int getCost() {
        return (int)(data.cost * Math.Pow((1 + data.costFactor), data.level));
    }
}
public struct UpgradeData {
    public String name;
    public String description;
    public String hovertext;
    public int    cost;
    /**
     * lvl 0 is disabled
     */
    public int         level;
    public int         maxLevel;
    public int         tier;
    public int         priority;
    public Texture2D   icon;
    public float       costFactor;
}

public class TestUpgrade : Upgrade {
    public override void init() {
        base.init();
        data.name        = "Test";
        data.description = "Test description";
        data.hovertext   = "Test hovertext";
        data.cost        = 1;
        data.maxLevel    = 5;
        data.icon        = new PlaceholderTexture2D();
    }
}
public class ScareUpgrade : Upgrade {
    private Player2 p;
    private Map m;
    public ScareUpgrade(Player2 player, Map m) {
        this.p = player;
        this.m = m;
    }
    public override void init() {
        base.init();
        data.name        = "Scary Mask";
        data.description = "Scare faster, +25% per level";
        data.hovertext   = "Its time to get spooky";
        data.cost        = 1;
        data.maxLevel    = -1;
        data.costFactor  = 0.125f;
        data.icon        = GD.Load<Texture2D>("res://oni2.png");

    }

    public override void update(float delta) {
        base.update(delta);
        p.scareRate      = 1f + (float)data.level * (0.25f);
        data.description = "Scare faster and when seen\n +25% speed per level\nCurrently: " + (1f + (float)data.level * (0.25f)) * 100 + "%";
    }

    public override void upgrade() {
        base.upgrade();
        if (data.level > 0) {
            foreach (Child3 c in m.getAllChildren()) {
                c.cantScareWhenSeen = false;
            }
        }
    }
}
public class HardUpgrade : Upgrade {
    private Player2 p;
    private Map     m;
    public HardUpgrade(Player2 player, Map m) {
        this.p = player;
        this.m = m;
    }
    public override void init() {
        base.init();
        data.name        = "Late Night";
        // data.description = ;
        data.hovertext = "The kids are too tired to be scared, but they have more candy";
        data.cost      = 5;
        data.maxLevel  = -1;
        data.icon      = GD.Load<Texture2D>("res://clock1.png");
    }

    public override void update(float delta) {
        base.update(delta);
        p.scareRate      = p.scareRate / (float)Math.Pow(2f, data.level);
        data.description = "kids take "+(float)Math.Pow(2f, data.level)+"X the time to scare, but drop "+(float)Math.Pow(2f, data.level)+"X the candy\ndoubles per level";
    }
    public override void upgrade() {
        base.upgrade();
        if (data.level > 0) {
            foreach (Child3 c in m.getAllChildren()) {
                c.candyMult = (float)Math.Pow(2f, data.level);
            }
        }
    }

}