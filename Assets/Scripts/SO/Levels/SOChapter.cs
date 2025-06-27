using UnityEngine;


[CreateAssetMenu(fileName = "SOChapter", menuName = "ScriptableObjects/Levels/SOChapter")]
public class SOChapter : ScriptableObject
{
    [SerializeField] string title;
    [SerializeField] GameObject[] levels;


    public string Title
    {
        get => title;
    }

    public int Size
    {
        get => levels.Length;
    }


    public GameObject GetLevelPrefab(int index)
    {
        return levels.Get(index);
    }

    public int GetLastIndex()
    {
        return levels.GetLastIndex();
    }
}
