using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuHolder;
    [SerializeField] Transform levelHolder;

    [SerializeField] TextMeshProUGUI
        text_storyActivitiesCompleted,
        text_enemiesKilled,
        text_chestLooted;

    InputSystemActions inputActions;
    LevelManager levelManager;
    LevelSaver levelSaver;
    Counter counter;


    public bool IsPaused { get; private set; }


    //init

    private void Awake()
    {
        inputActions = new InputSystemActions();
        levelManager = FindAnyObjectByType<LevelManager>();
        levelSaver = FindAnyObjectByType<LevelSaver>();
        counter = FindAnyObjectByType<Counter>();

        IsPaused = false;
        inputActions.Player.Pause.performed += Pause;
    }


    //clear

    private void OnDestroy()
    {
        inputActions.Player.Pause.performed -= Pause;
    }


    //public methods

    void Pause(InputAction.CallbackContext context)
    {
        if (!GameMarks.CanPlayerPauseGame)
        {
            return;
        }

        if (IsPaused)
        {
            return;
        }

        IsPaused = true;

        Time.timeScale = 0.0f;
        GameMarks.SetAll(false);
        pauseMenuHolder.SetActive(true);   

        if (levelSaver.SaveMode == LevelSaver.SaveModeEnum.InGame)
        {
            levelSaver.SaveLevel();
        }

        StartCoroutine(LoadData());
    }

    public void Resume()
    {
        pauseMenuHolder.SetActive(false);
        counter.Call(timeInSeconds: 2,
                     timeScaleAfter: 1.0f,
                     afterCountingAction: () => { IsPaused = false; });
    }

    public void ExitButton()
    {
        SceneManager.ChangeScene(Scene.MainMenu);
        Time.timeScale = 1.0f;
    }


    //private methods

    private IEnumerator LoadData()
    {
        LevelData levelData = levelManager.ActiveLevel;
        text_storyActivitiesCompleted.text = $"{levelData.StoryActivitiesCompleted}/{levelData.StoryActivitiesCount}";
        text_enemiesKilled.text = $"{levelData.EnemiesKilledFixed}/{levelData.EnemiesCount}";
        text_chestLooted.text = $"{levelData.ChestsLooted}/{levelData.ChestsCount}";
        yield return null;
    }
}
