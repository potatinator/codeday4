using Godot;
using System;

public interface upgrade {
    public void        init();
    public void        update(float delta);
    public void        upgrade();
    public upgradeData getData();
}
public struct upgradeData {
    private String      name;
    private String      description;
    private String      hovortext;
    private int         cost;
    private int         level;
    private int         maxLevel;
    private upgradeType type;
    private int         tier;
    private int         priority;
}
public enum upgradeType {
    TEST
}

public class TestUpgrade : upgrade {
    public void init() {
    }

    public void update(float delta) {
    }

    public void upgrade() {
    }

    public upgradeData getData() {
        return new upgradeData();
    }
}