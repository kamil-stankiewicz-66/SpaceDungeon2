using UnityEngine;

[CreateAssetMenu(fileName = "SOChapters", menuName = "ScriptableObjects/Levels/SOChapters")]
public class SOChapters : ScriptableObject
{
    [SerializeField] SOChapter[] chapters;

    public SOChapter[] Get => chapters;

    public int Size
    {
        get => chapters.Length;
    }
}
