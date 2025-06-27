using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOWeaponBase", menuName = "ScriptableObjects/DataObjects/WeaponsBase")]
public class SOWeaponsBase : ScriptableObject 
{
    [SerializeField] private GameObject[] weapons;
    
    Dictionary<int, bool> weaponsStatusData;
    

    public bool IsLoaded { get; set; }


    //only get
    
    public Dictionary<int, bool> WeaponsStatusData 
    {
        get => weaponsStatusData; 
    }

    public int WeaponsCount
    {
        get => weapons.Length;
    }

    
    //public methods

    public GameObject GetWeaponPrefab(int id)
    {
        if (id < 0 || weapons.Length <= id)
        {
            Debug.LogWarning($"SO_WeapondBase: id out of range");
            return null;
        }

        return weapons[id];
    }

    public Weapon GetWeaponCore(int id)
    {
        Weapon _w = GetWeaponPrefab(id)?.GetComponentInChildren<Weapon>();

        if (_w == null)
        {
            Debug.LogWarning("SO_WeapondBase: weapon is null");
        }

        return _w;
    }

    public bool GetWeaponStatus(int weaponNumber)
    {
        if (weaponsStatusData.TryGetValue(weaponNumber, out bool _value))
        {
            return _value;
        }

        return false;
    }

    public void SetWeaponStatus(int _id, bool _value = true)
    {
        if (weaponsStatusData.ContainsKey(_id))
        {
            weaponsStatusData[_id] = _value;
        }
        else
        {
            weaponsStatusData.Add(_id, _value);
        }
    }



    //load and save

    public void Load()
    {
        if (IsLoaded)
        {
            return;
        }

        weaponsStatusData = new Dictionary<int, bool>();

        if (Serializer.LoadBin(PATH.GetDirectory(PATH.WEAPONSBASE_FILE), out Struct_WeaponsBaseData data))
        {
            this.weaponsStatusData = data.weaponsStatusData;
            Debug.Log("SOWeaponsBase: Weapons base data loaded.");
        }
        else
        {
            Debug.Log("SOWeaponsBase: Weapons base data set default.");
        }

        IsLoaded = true;
    }

    public void Save()
    {
        Struct_WeaponsBaseData dataCopy = new Struct_WeaponsBaseData
        {
            weaponsStatusData = this.weaponsStatusData
        };

        dataCopy.SaveBin(PATH.GetDirectory(PATH.WEAPONSBASE_FILE));
        Debug.Log("SOWeaponsBase: WeaponsBase saved.");
    }

}
