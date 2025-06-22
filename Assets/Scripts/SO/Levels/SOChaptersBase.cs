using UnityEngine;

[CreateAssetMenu(fileName = "SOChapters", menuName = "ScriptableObjects/Levels/SOChapters")]
public class SOChaptersBase : ScriptableObject
{
    [SerializeField] SOChapter[] chapters;


    public bool IsLoaded { get; set; }



    public int Size
    {
        get => chapters.Length;
    }

    public SOChapter Get(int index) => chapters.Get(index);



    public void SetDefault()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < chapters.Get(i).Size; j++)
            {
                chapters.Get(i).Get(j).State = ELevelState.Default;
            }
        }

        IsLoaded = false;
    }
}
