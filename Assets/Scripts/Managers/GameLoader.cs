using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public static bool isGameLoading = true;

    [SerializeField] Transform player;
    [SerializeField] Transform levelHolder;

    [SerializeField] LevelSaver levelSaver;
    [SerializeField] Counter counter;

    [SerializeField] SOWeaponsBase SO_weaponsBase;
    [SerializeField] SOPlayerData SO_playerData;
    [SerializeField] SOGameStartupPackage SO_GamestartupPackage;
    [SerializeField] SOChapters SO_Chapters;

    private LevelData levelData;


    private void Awake()
    {
        //level load
        Instantiate(SO_Chapters.Get(SO_GamestartupPackage.Chapter).Get(SO_GamestartupPackage.Level).Get, levelHolder);

        //scripts
        if (levelData == null)
            levelData = levelHolder.GetComponentInChildren<LevelData>();
        HealthSystemPlayer playerHealthSystem = player.gameObject.GetComponent<HealthSystemPlayer>();

        //player max health from player data
        playerHealthSystem.Health_max = SO_playerData.Health;
        playerHealthSystem.HealStamina_max = SO_playerData.Health;

        //load weapon
        PlayerAttack playerAttackCore = player.GetComponent<PlayerAttack>();
        playerAttackCore.CurrentWeapon = SO_weaponsBase.GetWeaponPrefab(SO_playerData.WeaponID);

        //load level from bin files or set def
        LoadLevel(SO_GamestartupPackage.RunMode);

        //set savemode
        switch (SO_GamestartupPackage.RunMode)
        {
            case EGameRunMode.Start:
            case EGameRunMode.Continue:
                levelSaver.SaveModeChange(LevelSaver.SaveModeEnum.InGame);
                break;

            case EGameRunMode.Maxing:
                levelSaver.SaveModeChange(LevelSaver.SaveModeEnum.Disable);
                break;
        }

        //game loaded
        isGameLoading = false;
        print("GameLoader: Level loaded in run mode: " + SO_GamestartupPackage.RunMode);
    }

    private void Start()
    {
        if (levelData == null)
            levelData = levelHolder.GetComponentInChildren<LevelData>();

        if (SO_GamestartupPackage.RunMode != EGameRunMode.Continue)
        {
            GameMarks.TurnAllOn();
            return;
        }
        else
        {
            counter.Call(3, 1.0f);
        }
    }


    /// <summary>
    /// level loader
    /// </summary>

    private void LoadLevel(EGameRunMode _runMode)
    {
        //using
        HealthSystemPlayer playerHealthSystem = player.gameObject.GetComponent<HealthSystemPlayer>();
        QuestManager questManager = FindAnyObjectByType<QuestManager>().GetComponent<QuestManager>();


        //player
        switch (_runMode)
        {
            case EGameRunMode.Start:
            case EGameRunMode.Maxing:
                {
                    playerHealthSystem.Health = playerHealthSystem.Health_max;
                    playerHealthSystem.HealStamina = 0.0f;
                    player.transform.position = levelData.Spawnpoint;
                    break;
                }            
        }


        //story activities
        for (ushort i = 0; i < levelData.StoryActivities.Length; i++)
        {
            StoryActivity storyActivity = levelData.StoryActivities[i];
            
            //1
            switch (_runMode)
            {
                case EGameRunMode.Start:
                {
                    storyActivity.IsActivityCompleted = false;
                    break;
                }

                //case EGameRunMode.Continue:
                //    {
                //        if (Serializer.LoadBin(PATH.LEVEL_STORYACTIVITY_FILE(so_levelsBase.currentLevelPointer, i), 
                //            out Struct_StroryActivity storyData))
                //        {
                //            storyActivity.IsActivityCompleted = storyData.isCompleted;
                //        }
                //        else
                //        {
                //            storyActivity.IsActivityCompleted = false;
                //        }
                //        break;
                //    }

                case EGameRunMode.Maxing:
                {
                    storyActivity.IsActivityCompleted = true;
                    break;
                }
            }

            //2
            storyActivity.Quest_id = i;
            if (!storyActivity.IsActivityCompleted)
            {
                questManager.CreateNewQuestMark(i, storyActivity);
            }

        }


        //enemies
        for (ushort i = 0; i < levelData.Enemies.Length; i++)
        {
            Entity enemie = levelData.Enemies[i];
            Transform _t = enemie.gameObject.transform;
            HealthSystemEntity healthSystem = enemie.HealthSystem;

            switch (_runMode)
            {
                case EGameRunMode.Start:
                    {
                        healthSystem.Health = healthSystem.Health_max;
                        break;
                    }

                //case EGameRunMode.Continue:
                //    {
                //        if (Serializer.LoadBin(PATH.LEVEL_ENEMY_FILE(so_levelsBase.currentLevelPointer, i), out Struct_Enemie enemieData))
                //        {
                //            _t.position = new Vector2(enemieData.position_x, enemieData.position_y);
                //            healthSystem.Health = enemieData.health;
                //        }
                //        else
                //        {
                //            healthSystem.Health = healthSystem.Health_max;
                //        }
                //        break;
                //    }

                //case EGameRunMode.Maxing:
                //    {
                //        healthSystem.Health = healthSystem.Health_max;
                //        if (Serializer.LoadBin(PATH.LEVEL_ENEMY_FILE(so_levelsBase.currentLevelPointer, i), out Struct_Enemie enemieData))
                //        {
                //            if (enemieData.health <= 0f)
                //                enemie.IsRespawnedInMaxingMode = true;
                //        }
                //        break;
                //    }
            }

        }


        //chests
        for (ushort i = 0; i < levelData.Chests.Length; i++)
        {
            ChestCore chest = levelData.Chests[i];
            
            switch (_runMode)
            {
                case EGameRunMode.Start:
                    {
                        chest.IsLooted = false;
                        break;
                    }

                //case EGameRunMode.Continue:
                //    {
                //        if (Serializer.LoadBin(PATH.LEVEL_CHEST_FILE(so_levelsBase.currentLevelPointer, i), out Struct_Chest chestData))
                //        {
                //            chest.IsLooted = chestData.isLooted;
                //        }
                //        else
                //        {
                //            chest.IsLooted = false;
                //        }
                //        break;
                //    }

                //case EGameRunMode.Maxing:
                //    {
                //        if (Serializer.LoadBin(PATH.LEVEL_CHEST_FILE(so_levelsBase.currentLevelPointer, i), out Struct_Chest chestData))
                //        {
                //            chest.IsLooted = chestData.isLooted;
                //        }
                //        else
                //        {
                //            chest.IsLooted = false;
                //        }
                //        chest.IsRespawnedInMaxingMode = chest.IsLooted;
                //        break;
                //    }
            }
        }
    }

}
