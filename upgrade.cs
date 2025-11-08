using Godot;
using System;
using System.Transactions;

public class Upgrade {
    public UpgradeData data;

    public virtual void init() {
        data.level       = 0;
        data.costFactor  = 1f;
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
        return (int)((data.cost * (data.level + 1) * data.costFactor) - ((data.cost*(data.level + 1) * data.costFactor) % 5));
    }

    public int getRoundCost(int cost) {
        return (cost - (cost % 5));
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
        data.cost        = 15;
        data.maxLevel    = -1;
        data.costFactor  = 0.1f;
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

    public override int getCost() {
        return getRoundCost((int)(Math.Ceiling(data.cost + (Math.Pow((1.25*(data.level)), 2)) + (2.75*(data.level+1)))));
    }
}
public class HardUpgrade : Upgrade {
    private Player2 p;
    public HardUpgrade(Player2 player) {
        this.p = player;
    }
    public override void init() {
        base.init();
        data.name        = "Late Night";
        // data.description = ;
        data.hovertext  = "Hardcore trick-or-treaters are hard to scare, but they have more candy";
        data.cost       = 50;
        data.maxLevel   = -1;
        data.costFactor = 1;
        data.icon       = GD.Load<Texture2D>("res://clock1.png");
    }

    public override int getCost() {
        return getRoundCost((int)(data.cost + Math.Pow((12.5*data.level), 2) + (12.5*data.level)));
    }

    public override void update(float delta) {
        base.update(delta);
        p.scareRate      = p.scareRate / (float)Math.Ceiling(0.5*(Math.Pow((data.level+1), 1.5)));
        p.candyMult = (float)(int)(Math.Pow((data.level+1), 1.5f)+1);
        data.description = "kids take "+(float)(float)Math.Ceiling(0.5*(Math.Pow((data.level+1), 1.5)))+"X the time to scare, but drop "+(float)(int)(Math.Pow((data.level+1), 1.5f)+1)+"X the candy\ndoubles per level";
    }

}
public class BribeUpgrade : Upgrade {
    private Player2 p;
    public BribeUpgrade(Player2 player) {
        this.p = player;
    }
    public override void init() {
        base.init();
        data.name        = "Bribe";
        data.description = "Gets you another bribe";
        data.hovertext   = "Keeps those pesky cops away";
        data.cost        = 1;
        data.maxLevel    = -1;
        data.costFactor  = 0.1f;
        data.icon        = new PlaceholderTexture2D();
        
    }

    public override int getCost() {
        return getRoundCost((int)(50 + (50*p.lives)));
            
    }

    public override void upgrade() {
        p.lives++;
    }
}