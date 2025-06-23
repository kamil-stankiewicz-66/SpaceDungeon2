using UnityEngine;

public class GameStateMonitor : MonoBehaviour
{
    [SerializeField] SystemLogCaller logCaller;

    InputManager inputManager;
    StatsViewPanel statsViewPanel;
    PlayerCore playerCore;


    private void Awake()
    {
        inputManager = FindAnyObjectByType<InputManager>();
        statsViewPanel = FindAnyObjectByType<StatsViewPanel>();
        playerCore = FindAnyObjectByType<PlayerCore>();
    }



    private void FixedUpdate()
    {
        //if player dead

        if (playerCore.HealthSystem.IsDead)
        {
            statsViewPanel.Display(StatsViewPanel.EPanelMode.DeadScreen);
        }

        
        //pause

        if (GameMarks.CanPlayerPauseGame && inputManager.PauseTrigger)
        {
            statsViewPanel.Display(StatsViewPanel.EPanelMode.Pause);
            logCaller.ShowLog("Paused");
        }
    }
}
