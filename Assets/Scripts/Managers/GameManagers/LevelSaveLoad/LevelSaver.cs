using UnityEngine;



public class LevelSaver : MonoBehaviour
{
    [SerializeField] SystemLogCaller systemLogCaller;

    PlayerCore player;
    LevelManager levelManager;


    public ESaveMode SaveMode { get; private set; }



    //public

    public enum ESaveMode
    {
        Disable, InGame, PlayerDead, PlayerDeadInMaxingMode, LevelCompleted
    }

    public void ChangeSaverMode(ESaveMode _saveMode)
    {
        if (_saveMode == SaveMode)
        {
            return;
        }

        SaveMode = _saveMode;
        print("LevelSaver: SaveMode changed to: " + SaveMode);
    }


    //private methods

    private void Start()
    {
        player = FindAnyObjectByType<PlayerCore>();
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnDestroy()
    {
        SaveMode = default;
        print("LevelSaver: OnDestroy: set level save mode on default: " + SaveMode);
    }


    //Save level

    public void SaveLevel()
    {
        switch (SaveMode)
        {
            case ESaveMode.Disable:
            {
                systemLogCaller.ShowLog("Save is disabled in this mode.");
                break;
            }

            case ESaveMode.InGame:
            {
                Save_PlayerData();
                Save_StoryActivities();
                Save_Enemies();
                Save_Chests();
                Save_LevelMeta();
                systemLogCaller.ShowLog("Level saved.");
                break;
            }

            case ESaveMode.PlayerDead:
            {
                Save_PlayerData(false);
                Save_StoryActivities(false);
                Save_Enemies(false);
                Save_Chests();
                Save_LevelMeta(0, 0, null);
                systemLogCaller.ShowLog("Level saved.");
                break;
            }

            case ESaveMode.PlayerDeadInMaxingMode:
            {
                Save_Chests();
                Save_LevelMeta();
                systemLogCaller.ShowLog("Level saved.");
                break;
            }

            case ESaveMode.LevelCompleted:
            {
                Save_PlayerData(false);
                Save_StoryActivities();
                Save_Enemies();
                Save_Chests();
                Save_LevelMeta();
                systemLogCaller.ShowLog("Level saved.");
                break;
            }
        }

        print("LevelSaver: Level saved in save mode: " + SaveMode);
    }

    private void Save_PlayerData(bool save = true)
    {
        //player
        PlayerHealthSystem playerHealthSystem = player.gameObject.GetComponent<PlayerHealthSystem>();
        Struct_Player playerData = new Struct_Player
        {
            position_x = player.transform.position.x,
            position_y = player.transform.position.y,
            health = playerHealthSystem.Health,
            healPoints = playerHealthSystem.HealPoints
        };

        string _path = PATH.GetDirectory(new string[]
        {
            PATH.LEVELS_FOLDER,
            levelManager.ActiveLevelPointer.Item1.ToString(),
            levelManager.ActiveLevelPointer.Item2.ToString(),
            PATH.LEVELS_PLAYERDATA_FILE
        });

        playerData.SaveBin(_path);

        //make error
        if (!save)
        {
            int _i = -1;
            _i.SaveBin(_path);
        }
    }

    private void Save_StoryActivities(bool save = true)
    {
        print("LevelSaver: num of existing story activities " + levelManager.ActiveLevel.StoryActivitiesCount);

        //story
        for (ushort i = 0; i < levelManager.ActiveLevel.StoryActivitiesCount; i++)
        {
            StoryActivity storyActivity = levelManager.ActiveLevel.StoryActivities[i];
            Struct_StroryActivity data = new Struct_StroryActivity
            {
                isCompleted = storyActivity.IsCompleted
            };

            string _path = PATH.GetDirectory(new string[] 
            { 
                PATH.LEVELS_FOLDER,
                levelManager.ActiveLevelPointer.Item1.ToString(),
                levelManager.ActiveLevelPointer.Item2.ToString(),
                PATH.LEVELS_STORYACTIVITY_FOLDER, 
                i.ToString() 
            });
            
            data.SaveBin(_path);

            //make error
            if (!save)
            {
                int _i = -1;
                _i.SaveBin(_path);
            }
        }
    }

    private void Save_Enemies(bool save = true)
    {
        print("LevelSaver: num of existing enemies " + levelManager.ActiveLevel.EnemiesCount);

        //enemies
        for (ushort i = 0; i < levelManager.ActiveLevel.EnemiesCount; i++)
        {
            EntityCore enemie = levelManager.ActiveLevel.Enemies[i];
            Transform _t = enemie.gameObject.transform;
            HealthSystem healthSystem = enemie.HealthSystem;

            if (enemie.IsRespawnedInMaxingMode)
            {
                continue;
            }

            Struct_Enemy data = new Struct_Enemy
            {
                position_x = _t.position.x,
                position_y = _t.position.y,
                health = healthSystem.Health
            };

            string _path = PATH.GetDirectory(new string[] 
            { 
                PATH.LEVELS_FOLDER,
                levelManager.ActiveLevelPointer.Item1.ToString(),
                levelManager.ActiveLevelPointer.Item2.ToString(),
                PATH.LEVELS_ENEMIES_FOLDER, 
                i.ToString() 
            });

            data.SaveBin(_path);

            //make error
            if (!save)
            {
                int _i = -1;
                _i.SaveBin(_path);
            }
        }
    }

    private void Save_Chests(bool save = true)
    {
        print("LevelSaver: num of existing chests " + levelManager.ActiveLevel.ChestsCount);

        //chests
        for (ushort i = 0; i < levelManager.ActiveLevel.ChestsCount; i++)
        {
            ChestCore chest = levelManager.ActiveLevel.Chests[i];
            Struct_Chest data = new Struct_Chest
            {
                isLooted = chest.IsLooted
            };

            string _path = PATH.GetDirectory(new string[] 
            { 
                PATH.LEVELS_FOLDER,
                levelManager.ActiveLevelPointer.Item1.ToString(),
                levelManager.ActiveLevelPointer.Item2.ToString(),
                PATH.LEVELS_CHESTS_FOLDER, i.ToString() 
            });

            data.SaveBin(_path);

            //make error
            if (!save)
            {
                int _i = -1;
                _i.SaveBin(_path);
            }
        }
    }

    public void Save_LevelMeta(int? _storyCompleted = null, int? _enemiesKilled = null, int? _chestsLooted = null)
    {
        //meta file
        Struct_LevelMeta levelMetaData = new Struct_LevelMeta
        {
            storyCompleted = _storyCompleted ?? levelManager.ActiveLevel.StoryActivitiesCompleted,
            enemiesKilled = _enemiesKilled ?? levelManager.ActiveLevel.EnemiesKilledFixed,
            chestsLooted = _chestsLooted ?? levelManager.ActiveLevel.ChestsLooted
        };

        string _path = PATH.GetDirectory(new string[]
        {
            PATH.LEVELS_FOLDER,
            levelManager.ActiveLevelPointer.Item1.ToString(),
            levelManager.ActiveLevelPointer.Item2.ToString(),
            PATH.LEVELS_META_FILE
        });

        levelMetaData.SaveBin(_path);
    }

}
