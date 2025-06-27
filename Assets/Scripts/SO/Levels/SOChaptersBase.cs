using System.Collections.Generic;
using UnityEngine;

public enum ELevelState { Default, Started, Completed }

[CreateAssetMenu(fileName = "SOChapters", menuName = "ScriptableObjects/Levels/SOChapters")]
public class SOChaptersBase : ScriptableObject
{
    [SerializeField] SOChapter[] chapters;

    //meta data
    Dictionary<(int, int), ELevelState> levelsMeta;


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
            int level = GetLocalCurrentOrLastLevelPtr(chapter);
            int chapterLastIndex = Get(chapter).GetLastIndex();

            //this is last level in last chapter
            if (chapter == lastChapterIndex && level == chapterLastIndex)
            {
                return (chapter, level);
            }

            //this is not last level in this chapter
            if (level < chapterLastIndex)
            {
                return (chapter, level);
            }

            //this is last level in this chapter
        }

        //return last
        SOChapter lastChapter = Get(lastChapterIndex);
        return (lastChapterIndex, lastChapter.GetLastIndex());
    }

    public int GetLocalCurrentOrLastLevelPtr(int chapter)
    {
        for (int ptr = 0; ptr < Size; ptr++)
        {
            if (GetLevelState(chapter, ptr) != ELevelState.Completed)
            {
                return ptr;
            }
        }

        return Get(chapter).GetLastIndex();
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


    public ELevelState GetLevelState(int chapter, int level)
    {
        if (levelsMeta.TryGetValue((chapter, level), out ELevelState state))
        {
            return state;
        }

        return ELevelState.Default;
    }

    public void SetLevelState(int chapter, int level, ELevelState state)
    {
        if (chapter < 0 || chapter > GetLastIndex())
        {
            Debug.LogWarning("SO_CHAPTERS_BASE :: SET_STATE :: chapter index out of range");
            return;
        }

        
        if (level < 0 || level > Get(chapter).GetLastIndex())
        {
            Debug.LogWarning("SO_CHAPTERS_BASE :: SET_STATE :: level index out of range");
            return;
        }


        (int, int) index = (chapter, level);

        if (levelsMeta.ContainsKey(index))
        {
            levelsMeta[index] = state;
        }
        else
        {
            levelsMeta.Add(index, state);
        }
    }


    //load and save

    public void Load()
    {
        if (IsLoaded)
        {
            return;
        }

        levelsMeta = new Dictionary<(int, int), ELevelState>();

        if (Serializer.LoadBin(PATH.GetDirectory(PATH.LEVELRUNMODE_FILE), out Struct_LevelBaseData data))
        {
            levelsMeta = data.levels_state_data;

            Debug.Log("SOChaptersBase: Level base data loaded.");
        }
        else
        {
            Debug.Log("SOChaptersBase: Level base data set default.");
        }

        IsLoaded = true;
    }

    public void Save()
    {
        Struct_LevelBaseData dataCopy = new Struct_LevelBaseData();
        dataCopy.levels_state_data = new Dictionary<(int, int), ELevelState>();
        dataCopy.levels_state_data = this.levelsMeta;

        dataCopy.SaveBin(PATH.GetDirectory(PATH.LEVELRUNMODE_FILE));

        Debug.Log("LS_LevelsBase: LevelsBase saved.");
    }
}

