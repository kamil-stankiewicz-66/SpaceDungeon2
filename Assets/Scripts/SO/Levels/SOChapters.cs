using UnityEngine;

[CreateAssetMenu(fileName = "SOChapters", menuName = "ScriptableObjects/Levels/SOChapters")]
public class SOChapters : ScriptableObject
{
    [SerializeField] SOChapter[] chapters;

    public int Size
    {
        get => chapters.Length;
    }

    public SOChapter Get(int index) => chapters.Get(index);
}
