using UnityEngine;

[System.Serializable]
public struct Struct_Player
{
    public float position_x;
    public float position_y;
    public float health;
    public float healStamina;
}

[System.Serializable]
public struct Struct_StroryActivity
{
    public bool isCompleted;
}

[System.Serializable]
public struct Struct_Enemie
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
public struct Struct_LevelInfo
{
    public int storyCompleted;
    public int enemiesKilled;
    public int chestsLooted;
}

public class LevelSaver : MonoBehaviour
{
    [SerializeField] SystemLogCaller systemLogCaller;
    [SerializeField] Transform player;
    [SerializeField] Transform levelHolder;
    private LevelData levelData;
    private SaveModeEnum saveMode;
    private SaveModeEnum preSaveMode;
    

    /// <summary>
    /// only get
    /// </summary>

    public SaveModeEnum SaveMode
    {
        get => saveMode;
    }

    public SaveModeEnum PreSaveMode
    {
        get => preSaveMode;
    }


    /// <summary>
    /// public
    /// </summary>

    public enum SaveModeEnum
    {
        Disable, InGame, PlayerDead, PlayerDeadInMaxingMode, LevelCompleted
    }

    public void SaveModeChange(SaveModeEnum _saveMode)
    {
        if (_saveMode == saveMode)
            return;

        preSaveMode = saveMode;
        saveMode = _saveMode;
        print("LevelSaver: SaveMode changed to: " + saveMode);
    }


    /// <summary>
    /// private methods
    /// </summary>

    private void Start()
    {
        levelData = levelHolder.GetComponentInChildren<LevelData>();
    }

    private void OnDestroy()
    {
        saveMode = default;
        print("LevelSaver: OnDestroy: set level save mode on default: " + saveMode);
    }


    /// <summary>
    /// Save level
    /// </summary>

    public void SaveLevel()
    {
        switch (saveMode)
        {
            case SaveModeEnum.Disable:
                {
                    systemLogCaller.ShowLog("Save is disabled in this mode.");
                    break;
                }

            case SaveModeEnum.InGame:
                {
                    Save_PlayerData();
                    Save_StoryActivities();
                    Save_Enemies();
                    Save_Chests();
                    Save_LevelInfo();
                    systemLogCaller.ShowLog("Level saved.");
                    break;
                }

            case SaveModeEnum.PlayerDead:
                {
                    Save_PlayerData(false);
                    Save_StoryActivities(false);
                    Save_Enemies(false);
                    Save_Chests();
                    Save_LevelInfo(0, 0, null);
                    systemLogCaller.ShowLog("Level saved.");
                    break;
                }

            case SaveModeEnum.PlayerDeadInMaxingMode:
                {
                    Save_Chests();
                    Save_LevelInfo();
                    systemLogCaller.ShowLog("Level saved.");
                    break;
                }

            case SaveModeEnum.LevelCompleted:
                {
                    Save_PlayerData(false);
                    Save_StoryActivities();
                    Save_Enemies();
                    Save_Chests();
                    Save_LevelInfo();
                    systemLogCaller.ShowLog("Level saved.");
                    break;
                }
        }

        print("LevelSaver: Level saved in save mode: " + saveMode);
    }

    private void Save_PlayerData(bool save = true)
    {
        ////level
        //Level level = so_levelsBase.currentLevelPointer;

        ////player
        //HealthSystemPlayer playerHealthSystem = player.gameObject.GetComponent<HealthSystemPlayer>();
        //Struct_Player playerData = new Struct_Player
        //{
        //    position_x = player.position.x,
        //    position_y = player.position.y,
        //    health = playerHealthSystem.Health,
        //    healStamina = playerHealthSystem.HealStamina
        //};

        //playerData.SaveBin(PATH.LEVEL_PLAYER_FILE(level));

        ////make error
        //if (!save)
        //{
        //    int _i = -1;
        //    _i.SaveBin(PATH.LEVEL_PLAYER_FILE(level));
        //}
    }

    private void Save_StoryActivities(bool save = true)
    {
        //print("LevelSaver: num of existing story activities " + levelData.StoryActivities.Length);

        ////level
        //Level level = so_levelsBase.currentLevelPointer;

        ////story
        //for (ushort i = 0; i < levelData.StoryActivities.Length; i++)
        //{
        //    StoryActivity storyActivity = levelData.StoryActivities[i];
        //    Struct_StroryActivity data = new Struct_StroryActivity
        //    {
        //        isCompleted = storyActivity.IsActivityCompleted
        //    };

        //    data.SaveBin(PATH.LEVEL_STORYACTIVITY_FILE(level, i));

        //    //make error
        //    if (!save)
        //    {
        //        int _i = -1;
        //        _i.SaveBin(PATH.LEVEL_STORYACTIVITY_FILE(level, i));
        //    }
        //}
    }

    private void Save_Enemies(bool save = true)
    {
        //print("LevelSaver: num of existing enemies " + levelData.Enemies.Length);

        ////level
        //Level level = so_levelsBase.currentLevelPointer;

        ////enemies
        //for (ushort i = 0; i < levelData.Enemies.Length; i++)
        //{
        //    Entity enemie = levelData.Enemies[i];
        //    Transform _t = enemie.gameObject.transform;
        //    HealthSystemEntity healthSystem = enemie.HealthSystem;

        //    if (enemie.IsRespawnedInMaxingMode)
        //    {
        //        continue;
        //    }

        //    Struct_Enemie data = new Struct_Enemie
        //    {
        //        position_x = _t.position.x,
        //        position_y = _t.position.y,
        //        health = healthSystem.Health
        //    };

        //    data.SaveBin(PATH.LEVEL_ENEMY_FILE(level, i));

        //    //make error
        //    if (!save)
        //    {
        //        int _i = -1;
        //        _i.SaveBin(PATH.LEVEL_ENEMY_FILE(level, i));
        //    }
        //}
    }

    private void Save_Chests(bool save = true)
    {
        //print("LevelSaver: num of existing chests " + levelData.Chests.Length);

        ////level
        //Level level = so_levelsBase.currentLevelPointer;

        ////chests
        //for (ushort i = 0; i < levelData.Chests.Length; i++)
        //{
        //    ChestCore chest = levelData.Chests[i];
        //    Struct_Chest data = new Struct_Chest
        //    {
        //        isLooted = chest.IsLooted
        //    };

        //    data.SaveBin(PATH.LEVEL_CHEST_FILE(level, i));

        //    //make error
        //    if (!save)
        //    {
        //        int _i = -1;
        //        _i.SaveBin(PATH.LEVEL_CHEST_FILE(level, i));
        //    }
        //}
    }

    public void Save_LevelInfo(int? _storyCompleted = null, int? _enemiesKilled = null, int? _chestsLooted = null)
    {
        ////level
        //Level level = so_levelsBase.currentLevelPointer;

        ////info file
        //Struct_LevelInfo levelInfoData = new Struct_LevelInfo
        //{
        //    storyCompleted = _storyCompleted ?? levelData.StoryActivitiesCompleted,
        //    enemiesKilled = _enemiesKilled ?? levelData.EnemiesKilledFixed,
        //    chestsLooted = _chestsLooted ?? levelData.ChestsLooted
        //};

        //levelInfoData.SaveBin(PATH.LEVEL_INFO_FILE(level));
    }

}
