using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Struct_LevelBaseData
{
    public Dictionary<(int, int), ELevelState> levels_state_data;
};

[System.Serializable]
public struct Struct_PlayerData
{
    public int weaponNr;
    public int coins;
    public int exp;
}

[System.Serializable]
public struct Struct_WeaponsBaseData
{
    public Dictionary<int, bool> weaponsStatusData;
};


public class LSDataObjects : MonoBehaviour
{
    [SerializeField] SOPlayerData SO_PlayerData;
    [SerializeField] SOWeaponsBase SO_WeaponsBase;
    [SerializeField] SOChaptersBase SO_ChaptersBase;

    static GameObject instance;

    private void Awake()
    {
        //singleton

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = gameObject;
        DontDestroyOnLoad(instance);


        //load data objects

        SO_PlayerData.Load();
        SO_WeaponsBase.Load();
        SO_ChaptersBase.Load();
    }

    private void OnDestroy()
    {
        //this is not original singleton

        if (instance != gameObject)
        {
            return;
        }


        //save data objects

        SO_PlayerData.Save();
        SO_WeaponsBase.Save();
        SO_ChaptersBase.Save();


        //reset flags

        SO_PlayerData.IsLoaded = false;
        SO_WeaponsBase.IsLoaded = false;
        SO_ChaptersBase.IsLoaded = false;
    }
}
