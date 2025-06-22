using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private LevelData levelData;
    private LevelFinalScreen levelFinalScreen;
    private bool m_trigger_used;

    private void Awake()
    {
        levelData = FindAnyObjectByType<LevelData>();
        levelFinalScreen = FindAnyObjectByType<LevelFinalScreen>();

        if (levelData == null) Debug.LogError("FINISH_POINT :: level data not found");
        if (levelFinalScreen == null) Debug.LogError("FINISH_POINT :: level final screen not found");
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
        levelFinalScreen?.ShowWindow(LevelFinalScreen.FinalScreenModeEnum.LevelCompleted);
        print("FinishPoint: lvl complete");
    }
    
    

}
