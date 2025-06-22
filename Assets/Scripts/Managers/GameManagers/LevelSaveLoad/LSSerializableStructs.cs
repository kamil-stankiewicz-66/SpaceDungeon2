[System.Serializable]
public struct Struct_Player
{
    public float position_x;
    public float position_y;
    public float health;
    public float healPoints;
}

[System.Serializable]
public struct Struct_StroryActivity
{
    public bool isCompleted;
}

[System.Serializable]
public struct Struct_Enemy
{
    public float position_x;
    public float position_y;
    public float health;
}

[System.Serializable]
public struct Struct_Chest
{
    public bool isLooted;
}

[System.Serializable]
public struct Struct_LevelMeta
{
    public int storyCompleted;
    public int enemiesKilled;
    public int chestsLooted;
}
