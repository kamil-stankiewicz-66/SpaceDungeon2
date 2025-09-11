using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public void Load(LevelManager lm, LevelData levelData, EGameRunMode runMode)
    {
        LoadPlayer(lm, levelData, runMode);
        LoadStory(lm, levelData, runMode);
        LoadEnemies(lm, levelData, runMode);
        LoadChests(lm, levelData, runMode);
        LoadInteractables(lm, levelData, runMode);
    }

    void LoadPlayer(LevelManager lm, LevelData levelData, EGameRunMode runMode)
    {
        //using
        PlayerCore player = FindAnyObjectByType<PlayerCore>();
        PlayerHealthSystem playerHealthSystem = player.gameObject.GetComponent<PlayerHealthSystem>();

        //player default
        playerHealthSystem.Health = lm.SO_PlayerData.Health;
        playerHealthSystem.HealPoints = lm.SO_PlayerData.Health;
        player.transform.position = levelData.Spawnpoint;

        //load eq
        player.EquipmentSystem.SetActiveItem(lm.SO_ItemRegistry.Get(lm.SO_PlayerData.ActiveItem).Core);
        foreach (string itemID in lm.SO_PlayerData.Equipment)
        {
            player.EquipmentSystem.AddItemToEquipment(lm.SO_ItemRegistry.Get(itemID));
        }
        
        player.EquipmentSystem.SetCoins(lm.SO_PlayerData.Coins);

        //bonus to weapon damage
        Weapon _w = player.EquipmentSystem.ActiveItem as Weapon;
        _w.AddBonusDamage(lm.SO_PlayerData.Damage);

        //player
        switch (runMode)
        {
            case EGameRunMode.Continue:
            {
                string _path = PATH.GetDirectory(new string[]
                {
                    PATH.LEVELS_FOLDER,
                    lm.ActiveLevelPointer.Item1.ToString(),
                    lm.ActiveLevelPointer.Item2.ToString(),
                    PATH.LEVELS_PLAYERDATA_FILE
                });

                if (Serializer.LoadBin(_path, out Struct_Player data))
                {
                    playerHealthSystem.Health = data.health;
                    playerHealthSystem.HealPoints = data.healPoints;
                    player.transform.position = new Vector2(data.position_x, data.position_y);
                }

                break;
            }
        }
    }

    void LoadStory(LevelManager lm, LevelData levelData, EGameRunMode runMode)
    {
        //using
        QuestManager questManager = FindAnyObjectByType<QuestManager>().GetComponent<QuestManager>();

        //story activities
        for (int i = 0; i < levelData.StoryActivities.Length; i++)
        {
            StoryActivity storyActivity = levelData.StoryActivities[i];

            string _path = PATH.GetDirectory(new string[]
            {
                PATH.LEVELS_FOLDER,
                lm.ActiveLevelPointer.Item1.ToString(),
                lm.ActiveLevelPointer.Item2.ToString(),
                PATH.LEVELS_STORYACTIVITY_FOLDER,
                i.ToString()
            });

            //1
            switch (runMode)
            {
                case EGameRunMode.Start:
                {
                    storyActivity.IsCompleted = false;
                    break;
                }

                case EGameRunMode.Continue:
                {

                    if (Serializer.LoadBin(_path, out Struct_StroryActivity storyData))
                    {
                        storyActivity.IsCompleted = storyData.isCompleted;
                    }
                    else
                    {
                        storyActivity.IsCompleted = false;
                    }
                    break;
                }

                case EGameRunMode.Maxing:
                {
                    storyActivity.IsCompleted = true;
                    break;
                }
            }

            //2
            storyActivity.QuestID = i;
            if (!storyActivity.IsCompleted)
            {
                questManager.CreateNewQuestMark(i, storyActivity);
            }

        }
    }

    void LoadEnemies(LevelManager lm, LevelData levelData, EGameRunMode runMode)
    {
        //enemies
        for (int i = 0; i < levelData.Enemies.Length; i++)
        {
            EntityCore enemy = levelData.Enemies[i];
            enemy.gameObject.SetActive(true);
            HealthSystem healthSystem = enemy.GetComponent<HealthSystem>();

            //set default item (weapon)
            enemy.EquipmentSystem?.SetActiveItem(enemy.EquipmentSystem.ActiveItem);

            string _path = PATH.GetDirectory(new string[]
            {
                PATH.LEVELS_FOLDER,
                lm.ActiveLevelPointer.Item1.ToString(),
                lm.ActiveLevelPointer.Item2.ToString(),
                PATH.LEVELS_ENEMIES_FOLDER,
                i.ToString()
            });

            switch (runMode)
            {
                case EGameRunMode.Start:
                {
                    break;
                }

                case EGameRunMode.Continue:
                {
                    if (Serializer.LoadBin(_path, out Struct_Enemy enemieData))
                    {
                        enemy.transform.position = new Vector2(enemieData.position_x, enemieData.position_y);
                        healthSystem.Health = enemieData.health;
                    }

                    break;
                }

                case EGameRunMode.Maxing:
                {
                    if (Serializer.LoadBin(_path, out Struct_Enemy enemyData))
                    {
                        if (enemyData.health <= 0f)
                            enemy.IsRespawnedInMaxingMode = true;
                    }

                    break;
                }
            }

        }
    }

    void LoadChests(LevelManager lm, LevelData levelData, EGameRunMode runMode)
    {
        //chests
        for (int i = 0; i < levelData.Chests.Length; i++)
        {
            ChestCore chest = levelData.Chests[i];

            string _path = PATH.GetDirectory(new string[]
            {
                PATH.LEVELS_FOLDER,
                lm.ActiveLevelPointer.Item1.ToString(),
                lm.ActiveLevelPointer.Item2.ToString(),
                PATH.LEVELS_CHESTS_FOLDER,
                i.ToString()
            });

            switch (runMode)
            {
                case EGameRunMode.Start:
                {
                    chest.IsLooted = false;
                    break;
                }

                case EGameRunMode.Continue:
                {
                    if (Serializer.LoadBin(_path, out Struct_Chest chestData))
                    {
                        chest.IsLooted = chestData.isLooted;
                    }
                    else
                    {
                        chest.IsLooted = false;
                    }

                    break;
                }

                case EGameRunMode.Maxing:
                {
                    if (Serializer.LoadBin(_path, out Struct_Chest chestData))
                    {
                        chest.IsLooted = chestData.isLooted;
                    }
                    else
                    {
                        chest.IsLooted = false;
                    }
                    chest.IsRespawnedInMaxingMode = chest.IsLooted;
                    break;
                }
            }
        }
    }

    void LoadInteractables(LevelManager lm, LevelData levelData, EGameRunMode runMode)
    {
        //interactables
        for (int i = 0; i < levelData.Interactables.Length; i++)
        {
            InteractableObject interactable = levelData.Interactables[i];

            string _path = PATH.GetDirectory(new string[]
            {
                PATH.LEVELS_FOLDER,
                lm.ActiveLevelPointer.Item1.ToString(),
                lm.ActiveLevelPointer.Item2.ToString(),
                PATH.LEVELS_INTERACTABLES_FOLDER,
                i.ToString()
            });

            switch (runMode)
            {
                case EGameRunMode.Start:
                case EGameRunMode.Maxing:
                {
                    interactable.State = -1;
                    break;
                }

                case EGameRunMode.Continue:
                {
                    if (Serializer.LoadBin(_path, out Struct_Interactable interactableData))
                    {
                        interactable.State = interactableData.state;
                    }
                    else
                    {
                        interactable.State = -1;
                    }

                    break;
                }
            }
        }
    }
}
