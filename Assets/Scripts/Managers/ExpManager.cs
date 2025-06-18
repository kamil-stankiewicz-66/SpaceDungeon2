using UnityEngine;

public class ExpManager : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] Transform levelHolder;
    [SerializeField] SOGameStartupPackage SO_GameStartupPackage;


    /// <summary>
    /// public methods
    /// </summary>

    public int ExpForLevel()
    {
        int _expForLevel = 0;
        LevelData levelData = levelHolder.GetComponentInChildren<LevelData>();

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
                    _expForLevel += levelData.EnemiesKilled == levelData.EnemiesAll 
                        ? PARAMETERS.ALL_LEVEL_ENEMIES_KILLED 
                        : 0;

                    //chest
                    _expForLevel += levelData.ChestsLooted * PARAMETERS.CHEST_LOOTED;
                    _expForLevel += levelData.ChestsLooted == levelData.ChestsAll
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
        if (playerCore.playerHealthSystem.Health <= 0)
            _expForLevel /= 10;

        print("ExpManager: Exp is has been counted: " + _expForLevel);
        return _expForLevel;
    }
}
