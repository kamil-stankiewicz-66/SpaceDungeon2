using System.Collections;
using TMPro;
using UnityEngine;

public class StatsViewPanel : MonoBehaviour
{
    public enum EPanelMode
    {
        Pause, LevelCompleted, DeadScreen
    }

    [SerializeField] SystemLogCaller logCaller;

    [SerializeField] SOGameStartupPackage SO_GameStartupPackage;
    [SerializeField] SOPlayerData SO_PlayerData;
    [SerializeField] SOChaptersBase SO_ChaptersBase;

    [SerializeField] GameObject playerUI;

    [SerializeField] GameObject background;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject pauseHeader, deadHeader, completeHeader;
    [SerializeField] GameObject deadDescription;
    [SerializeField] GameObject buttonsLeft, buttonsRight;

    [SerializeField]
    TextMeshProUGUI
        text_storyActivitiesCompleted,
        text_enemiesKilled,
        text_chestLooted,
        text_expInfo;

    [SerializeField] GameObject expInfoHolder;

    LevelManager levelManager;
    LevelSaver levelSaver;
    Counter counter;

    bool isUsed;


    //init

    private void Awake()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
        levelSaver = FindAnyObjectByType<LevelSaver>();
        counter = FindAnyObjectByType<Counter>();

        background.SetActive(false);
        mainPanel.SetActive(false);
    }



    //display

    public void Display(EPanelMode mode)
    {
        if (isUsed)
        {
            return;
        }

        isUsed = true;
        StartCoroutine(LoadWindow(mode));
    }

    IEnumerator LoadWindow(EPanelMode mode)
    {
        //display progress data

        SetMode(mode);
        Active(true);

        LevelData levelData = levelManager.ActiveLevel;

        text_storyActivitiesCompleted.text = $"{levelData.StoryActivitiesCompleted}/{levelData.StoryActivitiesCount}";
        text_enemiesKilled.text = $"{levelData.EnemiesKilledFixed}/{levelData.EnemiesCount}";
        text_chestLooted.text = $"{levelData.ChestsLooted}/{levelData.ChestsCount}";

        yield return null;


        //if not pause

        if (mode != EPanelMode.Pause)
        {
            //count
            int expForLevel = levelManager.CountExp();

            //display
            text_expInfo.text = $"+{expForLevel}";

            //add exp
            SO_PlayerData.AddExp(expForLevel);
        }



        //set level state to completed if completed

        if (mode == EPanelMode.LevelCompleted)
        {
            (int, int) levelPtr = levelManager.ActiveLevelPointer;

            SOChapter chapter = SO_ChaptersBase.Get(levelPtr.Item1);
            ELevelState state = chapter.GetLevelState(levelPtr.Item2);

            chapter.SetLevelState(levelPtr.Item2, ELevelState.Completed);
        }



        //set save mode

        if (mode == EPanelMode.LevelCompleted)
        {
            levelSaver.ChangeSaverMode(LevelSaver.ESaveMode.LevelCompleted);
        }
        else if (mode == EPanelMode.DeadScreen)
        {
            if (SO_GameStartupPackage.RunMode == EGameRunMode.Maxing)
            {
                levelSaver.ChangeSaverMode(LevelSaver.ESaveMode.PlayerDeadInMaxingMode);
            }
            else
            {
                levelSaver.ChangeSaverMode(LevelSaver.ESaveMode.PlayerDead);
            }
        }



        //auto save

        levelSaver.SaveLevel();



        //set active

        yield return new WaitForSeconds(3);
        mainPanel.SetActive(true);
    }



    //helpers

    void SetMode(EPanelMode mode)
    {
        pauseHeader.SetActive(mode == EPanelMode.Pause);
        deadHeader.SetActive(mode == EPanelMode.DeadScreen);
        completeHeader.SetActive(mode == EPanelMode.LevelCompleted);

        deadDescription.SetActive(mode == EPanelMode.DeadScreen &&
                                  SO_GameStartupPackage.RunMode != EGameRunMode.Maxing);

        buttonsLeft.SetActive(true);
        buttonsRight.SetActive(mode == EPanelMode.Pause);

        expInfoHolder.SetActive(mode != EPanelMode.Pause);
    }

    void Active(bool active)
    {
        playerUI.SetActive(!active);
        background.SetActive(active);
        GameMarks.SetAll(!active);
    }



    //buttons

    public void Resume()
    {
        Active(false);

        counter.Call(timeInSeconds: 2,
                     timeScaleAfter: 1.0f,
                     afterCountingAction: () => { isUsed = false; logCaller.ShowLog("Resumed"); });
    }

    public void GotoMainScene()
    {
        isUsed = false;
        SceneManager.ChangeScene(Scene.MainMenu);
    }
}
