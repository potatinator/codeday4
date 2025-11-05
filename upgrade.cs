using Godot;
using System;
using System.Transactions;

public class Upgrade {
    public UpgradeData data;

    public void init() {
        data.level    = 0;
        data.priority = 1;
    }

    public void update(float delta) {
    }

    public void upgrade() {
        if (data.level < data.maxLevel) {
            data.level += 1;
        }
    }
}
public struct UpgradeData {
    public String      name;
    public String      description;
    public String      hovortext;
    public int         cost;
    public int         level;
    public int         maxLevel;
    public int         tier;
    public int         priority;
    public UpgradeType type;
    public Image       icon;
}
public enum UpgradeType {
    TEST
}

public class TestUpgrade : Upgrade {
    public new void init() {
        base.init();
        data.name        = "Test";
        data.description = "Test description";
        data.hovortext   = "Test hovortext";
        data.cost        = 1;
        data.maxLevel    = 5;
        data.type        = UpgradeType.TEST;
    }

    public void update(float delta) {
        base.update(delta);
    }

    public void upgrade() {
        base.upgrade();
        GD.Print("upgrade test");
    }

}