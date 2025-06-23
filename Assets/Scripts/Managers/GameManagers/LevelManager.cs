using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static bool isGameLoading = true;

    public SOPlayerData SO_playerData;
    public SOWeaponsBase SO_weaponBase;
    public SOGameStartupPackage SO_GameStartupPackage;
    public SOChaptersBase SO_Chapters;

    [SerializeField] Transform levelHolder;

    [SerializeField] LevelSaver levelSaver;
    [SerializeField] Counter counter;



    public LevelData ActiveLevel { get; private set; }

    public (int, int) ActiveLevelPointer {get; private set; }



    private void Start()
    {
        //load level data
        SOChapter chapter = SO_Chapters.Get(SO_GameStartupPackage.Chapter);
        GameObject levelPrefab = chapter.GetLevelPrefab(SO_GameStartupPackage.Level);
        ELevelState levelState = chapter.GetLevelState(SO_GameStartupPackage.Level);

        //load level
        Instantiate(levelPrefab, levelHolder);

        //set level state
        if (levelState == ELevelState.Default)
        {
            chapter.SetLevelState(SO_GameStartupPackage.Level, ELevelState.Started);
        }

        //scripts
        ActiveLevel = levelHolder.GetComponentInChildren<LevelData>();

        //pointer
        ActiveLevelPointer = (SO_GameStartupPackage.Chapter, SO_GameStartupPackage.Level);

        //error
        if (ActiveLevel == null )
        {
            Debug.LogError("LEVEL_MANAGER :: active level is null");
            return;
        }

        //try get loader
        LevelLoader levelLoader = GetComponent<LevelLoader>();
        if (levelLoader == null )
        {
            Debug.LogError("LEVEL_MANAGER :: level loader is null");
            return;
        }

        //load level from bin files or set def
        levelLoader.Load(this, ActiveLevel, SO_GameStartupPackage.RunMode);

        //set savemode
        switch (SO_GameStartupPackage.RunMode)
        {
            case EGameRunMode.Start:
            case EGameRunMode.Continue:
                levelSaver.ChangeSaverMode(LevelSaver.ESaveMode.InGame);
                break;

            case EGameRunMode.Maxing:
                levelSaver.ChangeSaverMode(LevelSaver.ESaveMode.Disable);
                break;
        }

        //game loaded
        isGameLoading = false;
        print("GameLoader: Level loaded in run mode: " + SO_GameStartupPackage.RunMode);


        if (SO_GameStartupPackage.RunMode != EGameRunMode.Continue)
        {
            GameMarks.SetAll(true);
        }
        else
        {
            counter.Call(3, 1.0f);
        }
    }



    public int CountExp()
    {
        LevelData levelData = ActiveLevel;
        PlayerCore playerCore = FindAnyObjectByType<PlayerCore>(FindObjectsInactive.Include);

        int _expForLevel = 0;

        switch (SO_GameStartupPackage.RunMode)
        {
            case EGameRunMode.Start:
            case EGameRunMode.Continue:
            {
                //story
                _expForLevel += levelData.IsLevelCompleted
                    ? PARAMETERS.ALL_LEVEL_STORY_ACTIVITIES_COMPLETED
                    : 0;

                //enemies
                _expForLevel += levelData.EnemiesKilled * PARAMETERS.ENEMIE_KILL;
                _expForLevel += levelData.EnemiesKilled == levelData.EnemiesCount
                    ? PARAMETERS.ALL_LEVEL_ENEMIES_KILLED
                    : 0;

                //chest
                _expForLevel += levelData.ChestsLooted * PARAMETERS.CHEST_LOOTED;
                _expForLevel += levelData.ChestsLooted == levelData.ChestsCount
                    ? PARAMETERS.ALL_LEVEL_CHEST_LOOTED
                    : 0;

                break;
            }

            case EGameRunMode.Maxing:
            {
                //enemies
                _expForLevel += levelData.EnemiesKilledFixedNegate * PARAMETERS.ENEMIE_KILL;

                //chest
                _expForLevel += levelData.ChestsLootedFixedNegate * PARAMETERS.CHEST_LOOTED;

                //finish in maxing mode bonus
                _expForLevel += PARAMETERS.FINISH_LEVEL_IN_MAXING_MODE;

                break;
            }
        }

        //dead antibonus
        if (playerCore.HealthSystem.IsDead)
        {
            _expForLevel /= 10;
        }

        print("ExpManager: Exp is has been counted: " + _expForLevel);
        return _expForLevel;
    }
}
