using System.Collections.Generic;
using UnityEngine;

public enum ELevelState { Default, Started, Completed }

[CreateAssetMenu(fileName = "SOChapter", menuName = "ScriptableObjects/Levels/SOChapter")]
public class SOChapter : ScriptableObject
{
    [SerializeField] string title;
    [SerializeField] GameObject[] levels;

    //meta data
    Dictionary<int, ELevelState> _levelsMeta;

    Dictionary<int, ELevelState> LevelsMeta
    {
        get
        {
            if (_levelsMeta == null)
            {
                _levelsMeta = new Dictionary<int, ELevelState>();
            }

            return _levelsMeta;
        }
    }


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

    public ELevelState GetLevelState(int index)
    {
        if (LevelsMeta.TryGetValue(index, out ELevelState state))
        {
            return state;
        }

        return ELevelState.Default;
    }

    public int GetLastIndex() => levels.GetLastIndex();

    public int GetCurrentOrLastLevelPtr()
    {
        for (int ptr = 0; ptr < Size; ptr++)
        {
            if (GetLevelState(ptr) != ELevelState.Completed)
            {
                return ptr;
            }
        }

        return GetLastIndex();
    }

    public void SetLevelState(int index, ELevelState state)
    {
        if (index < 0 || index > GetLastIndex())
        {
            Debug.LogWarning("SO_CHAPTER :: SET_STATE :: index out of range");
            return;
        }

        if (LevelsMeta.ContainsKey(index))
        {
            LevelsMeta[index] = state;
        }
        else
        {
            LevelsMeta.Add(index, state);
        }
    }
}
