using System.Collections;
using UnityEngine;
using TMPro;

public class LevelFinalScreen : MonoBehaviour
{
    [SerializeField] LevelSaver levelSaver;
    [SerializeField] ExpManager expManager;

    [SerializeField] SOPlayerData SO_playerData;
    [SerializeField] SOGameStartupPackage SO_GameStartupPackage;
    [SerializeField] SOChapters SO_Chapters;

    [SerializeField] Transform levelHolder;

    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject parentHolder;
    [SerializeField] GameObject mainHolder;
    [SerializeField] GameObject deadHeader;
    [SerializeField] GameObject deadDescription;
    [SerializeField] GameObject winHeader;

    [SerializeField]
    TextMeshProUGUI
        text_storyActivitiesCompleted,
        text_enemiesKilled,
        text_chestLooted,
        text_expInfo;

    private int expForLevel;
    private LevelData levelData;
    private bool isUsed;


    /// <summary>
    /// public
    /// </summary>

    public enum FinalScreenModeEnum
    {
        LevelCompleted,
        DeadScreen
    }

    public void ShowWindow(FinalScreenModeEnum _finalScreenMode)
    {
        if (isUsed)
            return;

        isUsed = true;
        GameMarks.TurnAllOff();
        parentHolder.SetActive(true);
        StartCoroutine(LoadWindow(_finalScreenMode));
        print("LevelFinalScreen: Final cor started");
    }

    public void GotoMenu()
    {        
        SceneManager.ChangeScene(Scene.MainMenu);
    }


    /// <summary>
    /// privs
    /// </summary>
    
    private IEnumerator LoadWindow(FinalScreenModeEnum _finalScreenMode)
    {
        //ui disable
        playerUI.SetActive(false);
        yield return null;

        //data loading
        if (levelData == null)
            levelData = levelHolder.GetComponentInChildren<LevelData>();

        text_storyActivitiesCompleted.text = $"{levelData.StoryActivitiesCompleted}/{levelData.StoryActivitiesAll}";
        text_enemiesKilled.text = $"{levelData.EnemiesKilledFixed}/{levelData.EnemiesAll}";
        text_chestLooted.text = $"{levelData.ChestsLooted}/{levelData.ChestsAll}";

        expForLevel = expManager.ExpForLevel();
        text_expInfo.text = $"+{expForLevel}";

        switch (_finalScreenMode)
        {
            case FinalScreenModeEnum.LevelCompleted:
                {
                    winHeader.SetActive(true);
                    deadHeader.SetActive(false);
                    break;
                }                

            case FinalScreenModeEnum.DeadScreen:
                {
                    if (SO_GameStartupPackage.RunMode == EGameRunMode.Maxing)
                    {
                        winHeader.SetActive(false);
                        deadHeader.SetActive(true);
                        deadDescription.SetActive(false);
                    }
                    else
                    {
                        winHeader.SetActive(false);
                        deadHeader.SetActive(true);
                        deadDescription.SetActive(true);
                    }
                    break;
                }
        }

        //ending operations
        SO_Chapters.Get(SO_GameStartupPackage.Chapter).Get(SO_GameStartupPackage.Level).State = ELevelState.Completed;
        SO_playerData.AddExp(expForLevel);

        switch (_finalScreenMode)
        {
            case FinalScreenModeEnum.LevelCompleted:
                levelSaver.SaveModeChange(LevelSaver.SaveModeEnum.LevelCompleted);
                break;

            case FinalScreenModeEnum.DeadScreen:
                {
                    if (SO_GameStartupPackage.RunMode == EGameRunMode.Maxing)
                    {
                        levelSaver.SaveModeChange(LevelSaver.SaveModeEnum.PlayerDeadInMaxingMode);
                    }
                    else
                    {
                        levelSaver.SaveModeChange(LevelSaver.SaveModeEnum.PlayerDead);
                    }
                    break;
                }
        }
        levelSaver.SaveLevel();

        //set active
        yield return new WaitForSeconds(3);
        mainHolder.SetActive(true);

        print("LevelFinalScreen: Final cor end");
    }
}
