using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Struct_LevelBaseData
{
    public Dictionary<(int, int), ELevelState> levels_state_data;
};

public class LS_ChaptersBase : MonoBehaviour
{
    [SerializeField] SOChaptersBase SO_chapters;

    private void Awake()
    {
        if (SO_chapters.IsLoaded)
        {
            return;
        }

        print("LS_LevelsBase: LevelBase loading started.");
        if (Serializer.LoadBin(PATH.GetDirectory(PATH.LEVELRUNMODE_FILE), out Struct_LevelBaseData data))
        {
            for (int i = 0; i < SO_chapters.Size; i++)
            {
                for (int j = 0; j < SO_chapters.Get(i).Size; j++)
                {
                    SO_chapters.Get(i).SetLevelState(j, data.levels_state_data[(i, j)]);
                }
            }

            print("LS_LevelsBase: Level base data loaded.");
        }
        else
        {
            SO_chapters.SetDefault();

            print("LS_LevelsBase: Level base data set default.");
        }

        SO_chapters.IsLoaded = true;
    }

    private void OnDestroy()
    {
        Struct_LevelBaseData dataCopy = new Struct_LevelBaseData();
        dataCopy.levels_state_data = new Dictionary<(int, int), ELevelState>();

        for (int i = 0; i < SO_chapters.Size; i++)
        {
            for (int j = 0; j < SO_chapters.Get(i).Size; j++)
            {
                dataCopy.levels_state_data[(i, j)] = SO_chapters.Get(i).GetLevelState(j);
            }
        }

        dataCopy.SaveBin(PATH.GetDirectory(PATH.LEVELRUNMODE_FILE));
        print("LS_LevelsBase: LevelsBase saved.");
    }

}
