using System.Collections;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] LevelSaver levelSaver;
    [SerializeField] Counter counter;

    [SerializeField] GameObject pauseMenuHolder;
    [SerializeField] Transform levelHolder;

    [SerializeField] TextMeshProUGUI
        text_storyActivitiesCompleted,
        text_enemiesKilled,
        text_chestLooted;

    private LevelData levelData;


    /// <summary>
    /// public methods
    /// </summary>

    public void PauseOn()
    {
        if (!GameMarks.CanPlayerPauseGame)
            return;

        Time.timeScale = 0.0f;
        GameMarks.TurnAllOff();
        pauseMenuHolder.SetActive(true);   

        if (levelSaver.SaveMode == LevelSaver.SaveModeEnum.InGame)
            levelSaver.SaveLevel();

        StartCoroutine(LoadData());
    }

    public void Resume()
    {
        pauseMenuHolder.SetActive(false);
        counter.Call(timeInSeconds: 2,
                     timeScaleAfter: 1.0f);
    }

    public void ExitButton()
    {
        SceneManager.ChangeScene(Scene.MainMenu);
        Time.timeScale = 1.0f;
    }


    /// <summary>
    /// private methods
    /// </summary>

    private IEnumerator LoadData()
    {
        if (levelData == null)
        {
            levelData = levelHolder.GetComponentInChildren<LevelData>();
        }

        text_storyActivitiesCompleted.text = $"{levelData.StoryActivitiesCompleted}/{levelData.StoryActivitiesAll}";
        text_enemiesKilled.text = $"{levelData.EnemiesKilledFixed}/{levelData.EnemiesAll}";
        text_chestLooted.text = $"{levelData.ChestsLooted}/{levelData.ChestsAll}";
        yield return null;
    }
}
