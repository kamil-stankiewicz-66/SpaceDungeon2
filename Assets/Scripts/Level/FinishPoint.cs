using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private LevelData levelData;
    private StatsViewPanel statsViewPanel;
    private bool m_trigger_used;

    private void Awake()
    {
        levelData = FindAnyObjectByType<LevelData>();
        statsViewPanel = FindAnyObjectByType<StatsViewPanel>();

        if (levelData == null) Debug.LogError("FINISH_POINT :: level data not found");
        if (statsViewPanel == null) Debug.LogError("FINISH_POINT :: StatsViewPanel not found");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //aby nie uruchomic zakonczenie kilkukrotnie
        if (m_trigger_used)
            return;

        //czy to napewno gracz
        if (!collision.CompareTag(TAG.PLAYER))
            return;

        //czy to napewno gracz
        if (!collision.gameObject.TryGetComponent<PlayerCore>(out _))
            return;

        //czy wszystkie zadania zostaly ukonczone
        if (!levelData.IsLevelCompleted)
            return;
        
        m_trigger_used = true;

        //complete level
        statsViewPanel?.Display(StatsViewPanel.EPanelMode.LevelCompleted);

        //log
        print("FinishPoint: lvl complete");
    }
    
    

}
