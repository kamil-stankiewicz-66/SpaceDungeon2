using UnityEngine;

[CreateAssetMenu(fileName = "SOGameStartupPackage", menuName = "ScriptableObjects/PackageObjects/SOGameStartupPackage")]
public class SOGameStartupPackage : ScriptableObject
{
    int m_chapter = -1;
    int m_level = -1;

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
}
