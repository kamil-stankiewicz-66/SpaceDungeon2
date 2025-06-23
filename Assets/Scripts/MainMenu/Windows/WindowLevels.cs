using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class WindowLevels : MonoBehaviour
{
    [SerializeField] SOPlayerData SO_playerData;
    [SerializeField] SOGameStartupPackage SO_GameStartupPackage;
    [SerializeField] SOChaptersBase SO_Chapters;

    [SerializeField] DialogWindowCaller dialogWindow;
    [SerializeField] NotificationCaller notificationCaller;

    [SerializeField] TextMeshProUGUI text_chapterNum;
    [SerializeField] TextMeshProUGUI text_levelNum;
    [SerializeField] TextMeshProUGUI text_playButton;

    [SerializeField] TextMeshProUGUI
        text_storyActivitiesCompleted,
        text_enemiesKilled,
        text_chestLooted;

    [SerializeField] Slider levelProgressBar;
    [SerializeField] TextMeshProUGUI text_completedStory;

    [SerializeField] GameObject button_nextLvl;
    [SerializeField] GameObject button_preLvl;

    [SerializeField] GameObject bar_play;
    [SerializeField] GameObject bar_locked;


    //public

    public void NextLevel()
    {
        if (!IsLocalNextLevelExist())
            return;

        SO_GameStartupPackage.Level++;
        RefreshWindow();
    }

    public void PreLevel()
    {
        if (!IsLocalPreLevelExist())
            return;

        SO_GameStartupPackage.Level--;
        RefreshWindow();
    }

    public void ButtonPlay()
    {
        if (SO_playerData.WeaponID < 0)
        {
            notificationCaller.Show("You don't have any weapon\nGo to the store and prepere for battle!");
            return;
        }

        if (!IsPreLevelCompleted())
        {
            Debug.Log("WindowLevels: previous level is not completed");
            notificationCaller.Show("Previous level is not completed.");
            return;
        }

        
        //refresh runmode
        SO_GameStartupPackage.RefreshRunMode(SO_Chapters);


        UnityEvent e = new UnityEvent();
        e.AddListener(() => SceneManager.ChangeScene(Scene.Game));
        e.AddListener(() =>
        {
            SOChapter chapter = SO_Chapters.Get(SO_GameStartupPackage.Chapter);
            ELevelState levelState = chapter.GetLevelState(SO_GameStartupPackage.Level);

            if (levelState == ELevelState.Default)
            {
                chapter.SetLevelState(SO_GameStartupPackage.Level, ELevelState.Started);
            }
        });

        dialogWindow.Show("Ready to fight?", e);
    }


    //private

    private void OnEnable()
    {
        int _lastLevelInThisChapter = SO_Chapters.Get(SO_GameStartupPackage.Chapter).GetCurrentOrLastLevelPtr();
        SO_GameStartupPackage.Level = _lastLevelInThisChapter;

        RefreshWindow();
    }

    private void RefreshWindow()
    {
        text_chapterNum.text = $"Chapter {SO_GameStartupPackage.Chapter +1}";
        text_levelNum.text = Extensions.CreateText(text_levelNum, SO_GameStartupPackage.Level +1);
        ButtonsRefresh();

        int storyActivitiesCompleted = 0;
        int enemiesKilled = 0;
        int chestsLooted = 0;


        //refresh runmode
        SO_GameStartupPackage.RefreshRunMode(SO_Chapters);


        //load data from info file

        string _path = PATH.GetDirectory(new string[]
        {
            PATH.LEVELS_FOLDER,
            SO_GameStartupPackage.Chapter.ToString(),
            SO_GameStartupPackage.Level.ToString(),
            PATH.LEVELS_META_FILE
        });

        if (Serializer.LoadBin(_path, out Struct_LevelMeta levelMetaData))
        {
            storyActivitiesCompleted = levelMetaData.storyCompleted;
            enemiesKilled = levelMetaData.enemiesKilled;
            chestsLooted = levelMetaData.chestsLooted;
            print("WindowLevels: level info loaded from file");
        }

        SOChapter chapter = SO_Chapters.Get(SO_GameStartupPackage.Chapter);
        GameObject levelPrefab = chapter.GetLevelPrefab(SO_GameStartupPackage.Level);

        LevelData levelData = levelPrefab.GetComponent<LevelData>();
        levelData.BuildLevelData();
        int storyActivitiesAll = levelData.StoryActivitiesCount;
        int enemiesAll = levelData.EnemiesCount;
        int chestsAll = levelData.ChestsCount;

        text_storyActivitiesCompleted.text = $"{storyActivitiesCompleted}/{storyActivitiesAll}";
        text_enemiesKilled.text = $"{enemiesKilled}/{enemiesAll}";
        text_chestLooted.text = $"{chestsLooted}/{chestsAll}";

        if (IsPreLevelCompleted())
        {
            ChangePlayBar(bar_play);
            text_playButton.text = SO_GameStartupPackage.RunMode.ToString();
        }
        else
        {
            ChangePlayBar(bar_locked);
        }

        //progress bar
        int levelActivitiesCompleted = storyActivitiesCompleted + enemiesKilled + chestsLooted;
        int levelsActivitiesAll = storyActivitiesAll + enemiesAll + chestsAll;
        levelProgressBar.value = levelActivitiesCompleted;
        levelProgressBar.maxValue = levelsActivitiesAll;

        if (storyActivitiesCompleted < storyActivitiesAll)
        {
            text_completedStory.text = "Completed story: No";
        }
        else
        {
            text_completedStory.text = "Completed story: Yes";
        }
    }

    private void ButtonsRefresh()
    {
        //next lvl
        button_nextLvl.SetActive(IsLocalNextLevelExist());

        //pre lvl
        button_preLvl.SetActive(IsLocalPreLevelExist());
    }

    private void ChangePlayBar(GameObject _bar)
    {
        bar_play.SetActive(_bar == bar_play);
        bar_locked.SetActive(_bar == bar_locked);
    }

    private bool IsPreLevelCompleted()
    {
        if (!IsPreLevelExist())
        {
            return true;
        }

        var preLevel = SO_Chapters.GetPreviousPtr(SO_GameStartupPackage.Chapter, SO_GameStartupPackage.Level);
        return SO_Chapters.Get(preLevel.Item1).GetLevelState(preLevel.Item2) == ELevelState.Completed;
    }

    private bool IsPreLevelExist()
    {
        var preLevelPtr = SO_Chapters.GetPreviousPtr(SO_GameStartupPackage.Chapter, SO_GameStartupPackage.Level);
        int preChapter = preLevelPtr.Item1;
        int preLevel = preLevelPtr.Item2;

        return SO_GameStartupPackage.Chapter != preChapter || SO_GameStartupPackage.Level != preLevel;
    }

    private bool IsLocalPreLevelExist()
    {
        return SO_GameStartupPackage.Level != 0;
    }

    private bool IsNextLevelExist()
    {
        var nextLevelPtr = SO_Chapters.GetNextPtr(SO_GameStartupPackage.Chapter, SO_GameStartupPackage.Level);
        int nextChapter = nextLevelPtr.Item1;
        int nextLevel = nextLevelPtr.Item2;

        return SO_GameStartupPackage.Chapter != nextChapter || SO_GameStartupPackage.Level != nextLevel;
    }

    private bool IsLocalNextLevelExist()
    {
        return SO_GameStartupPackage.Level < SO_Chapters.Get(SO_GameStartupPackage.Chapter).GetLastIndex();
    }
}
