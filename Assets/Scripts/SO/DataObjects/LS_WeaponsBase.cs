using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Struct_WeaponsBaseData
{
    public Dictionary<int, bool> weaponsStatusData;
};

public class LS_WeaponsBase : MonoBehaviour
{
    [SerializeField] SOWeaponsBase SO_WeaponsBase;

    private void Awake()
    {
        if (SO_WeaponsBase.IsLoaded)
        {
            return;
        }

        print("LS_WeaponsBase: WeaponsBase loading started.");

        SO_WeaponsBase.SetDefault();

        if (Serializer.LoadBin(PATH.GetDirectory(PATH.WEAPONSBASE_FILE), out Struct_WeaponsBaseData data))
        {
            SO_WeaponsBase.weaponsStatusData = data.weaponsStatusData;
            print("LS_WeaponsBase: Weapons base data loaded.");
        }
        else
        {
            SO_WeaponsBase.WeaponsStatusDictionaryBuild();
            print("LS_WeaponsBase: Weapons base data set default.");
        }

        SO_WeaponsBase.IsLoaded = true;
    }

    private void OnDestroy()
    {
        Struct_WeaponsBaseData dataCopy = new Struct_WeaponsBaseData
        {
            weaponsStatusData = SO_WeaponsBase.weaponsStatusData
        };

        dataCopy.SaveBin(PATH.GetDirectory(PATH.WEAPONSBASE_FILE));
        print("LS_WeaponsBase: WeaponsBase saved.");
    }

}
