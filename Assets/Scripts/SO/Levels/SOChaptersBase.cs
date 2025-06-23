using PlasticGui.WorkspaceWindow;
using System.Data;
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

    public int GetLastIndex() => chapters.GetLastIndex();



    public (int, int) GetCurrentOrLastLevelPtr()
    {
        int lastChapterIndex = GetLastIndex();

        for (int chapter = 0; chapter <= lastChapterIndex; chapter++)
        {
            SOChapter chapterData = Get(chapter);
            int level = chapterData.GetCurrentOrLastLevelPtr();

            //this is last level in last chapter
            if (chapter == lastChapterIndex && level == chapterData.GetLastIndex())
            {
                return (chapter, level);
            }

            //this is not last level in this chapter
            if (level < chapterData.GetLastIndex())
            {
                return (chapter, level);
            }

            //this is last level in this chapter
        }

        //return last
        SOChapter lastChapter = Get(lastChapterIndex);
        return (lastChapterIndex, lastChapter.GetLastIndex());
    }



    public (int, int) GetNextPtr(int chapter, int level)
    {
        int nextLevel = level + 1;
        int lastLevelInChapter = Get(chapter).GetLastIndex();
        int lastChapter = GetLastIndex();

        if (nextLevel <= lastLevelInChapter)
        {
            return (chapter, nextLevel);
        }

        if (chapter < lastChapter)
        {
            return (chapter + 1, 0);
        }

        return (lastChapter, lastLevelInChapter);
    }

    public (int, int) GetPreviousPtr(int chapter, int level)
    {
        if (level > 0)
        {
            return (chapter, level - 1);
        }

        if (chapter > 0)
        {
            int prevChapter = chapter - 1;
            int prevLevel = Get(prevChapter).GetLastIndex();
            return (prevChapter, prevLevel);
        }

        return (0, 0);
    }



    public void SetDefault()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < chapters.Get(i).Size; j++)
            {
                chapters.Get(i).SetLevelState(j, ELevelState.Default);
            }
        }

        IsLoaded = false;
    }
}

