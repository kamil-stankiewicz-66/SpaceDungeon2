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
        if (!IsNextLevelExist())
            return;

        SO_GameStartupPackage.Level++;
        RefreshWindow();
    }

    public void PreLevel()
    {
        if (!IsPreLevelExist())
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

        UnityEvent e = new UnityEvent();
        e.AddListener(() => SceneManager.ChangeScene(Scene.Game));
        e.AddListener(() =>
        {
            var level = SO_Chapters.Get(SO_GameStartupPackage.Chapter).Get(SO_GameStartupPackage.Level);

            if (level.State == ELevelState.Default)
            {
                level.State = ELevelState.Started;
            }
        });

        dialogWindow.Show("Ready to fight?", e);
    }


    //private

    private void OnEnable()
    {
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


        var SO_Level = SO_Chapters.Get(SO_GameStartupPackage.Chapter).Get(SO_GameStartupPackage.Level);

        LevelData levelData = SO_Level.Get.GetComponent<LevelData>();
        levelData.BuildLevelData();
        int storyActivitiesAll = levelData.StoryActivitiesCount;
        int enemiesAll = levelData.EnemiesCount;
        int chestsAll = levelData.ChestsCount;

        text_storyActivitiesCompleted.text = $"{storyActivitiesCompleted}/{storyActivitiesAll}";
        text_enemiesKilled.text = $"{enemiesKilled}/{enemiesAll}";
        text_chestLooted.text = $"{chestsLooted}/{chestsAll}";

        //run mode
        switch (SO_Level.State)
        {
            case ELevelState.Default:
                SO_GameStartupPackage.RunMode = EGameRunMode.Start;
                break;

            case ELevelState.Started:
                SO_GameStartupPackage.RunMode = EGameRunMode.Continue;
                break;

            case ELevelState.Completed:
                SO_GameStartupPackage.RunMode = EGameRunMode.Maxing;
                break;

            default:
                SO_GameStartupPackage.RunMode = default;
                break;
        }


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
            text_completedStory.text = "Completed story: No";
        else
            text_completedStory.text = "Completed story: Yes";
    }

    private void ButtonsRefresh()
    {
        //next lvl
        button_nextLvl.SetActive(IsNextLevelExist());

        //pre lvl
        button_preLvl.SetActive(IsPreLevelExist());
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

        return SO_Chapters.Get(SO_GameStartupPackage.Chapter).Get(SO_GameStartupPackage.Level).State == ELevelState.Completed;
    }

    private bool IsPreLevelExist()
    {
        return SO_GameStartupPackage.Level > 0;
    }

    private bool IsNextLevelExist()
    {
        if (SO_Chapters.Size == 0)
        {
            return false;
        }

        return SO_GameStartupPackage.Level < (SO_Chapters.Get(SO_GameStartupPackage.Chapter).Size -1);
    }
}
