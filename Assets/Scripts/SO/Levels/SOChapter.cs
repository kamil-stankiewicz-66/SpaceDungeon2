using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "SOChapter", menuName = "ScriptableObjects/Levels/SOChapter")]
public class SOChapter : ScriptableObject
{
    [SerializeField] string title;
    [SerializeField] SOLevel[] levels;

    public string Title
    {
        get => title;
    }

    public int Size
    {
        get => levels.Length;
    }

    public SOLevel Get(int index) => levels.Get(index);
}
