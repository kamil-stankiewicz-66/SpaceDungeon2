using Unity.Collections;
using UnityEngine;

public enum EGameRunMode { Start, Continue, Maxing }

[CreateAssetMenu(fileName = "SOGameStartupPackage", menuName = "ScriptableObjects/PackageObjects/SOGameStartupPackage")]
public class SOGameStartupPackage : ScriptableObject
{
    [SerializeField] int m_chapter;
    [SerializeField] int m_level;
    [SerializeField] EGameRunMode runMode;

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
        get => runMode;
        set => runMode = value;
    }
}
