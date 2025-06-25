using UnityEngine;

public enum EGameRunMode { Start, Continue, Maxing }

[CreateAssetMenu(fileName = "SOGameStartupPackage", menuName = "ScriptableObjects/PackageObjects/SOGameStartupPackage")]
public class SOGameStartupPackage : ScriptableObject
{
    [SerializeField] int m_chapter;
    [SerializeField] int m_level;
    [SerializeField] EGameRunMode m_mode;

    public int Chapter
    {
        get => m_chapter;
        set => m_chapter = value;
    }

    public int Level
    {
        get => m_level;
        set => m_level = value;
    }

    public EGameRunMode RunMode
    {
        get => m_mode;
    }


    public void RefreshRunMode(SOChaptersBase @base)
    {
        switch (@base.Get(m_chapter).GetLevelState(m_level))
        {
            case ELevelState.Default: m_mode = EGameRunMode.Start; break;
            case ELevelState.Started: m_mode = EGameRunMode.Continue; break;
            case ELevelState.Completed: m_mode = EGameRunMode.Maxing; break;

            default: m_mode = default; break;
        }
    }

}
