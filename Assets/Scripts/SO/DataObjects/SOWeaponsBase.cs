using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOWeaponBase", menuName = "ScriptableObjects/DataObjects/WeaponsBase")]
public class SOWeaponsBase : ScriptableObject 
{
    [HideInInspector] public Dictionary<int, bool> weaponsStatusData;
    [SerializeField] private GameObject[] weapons;


    public bool IsLoaded { get; set; }


    //only get
    
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

    public void WeaponsStatusDictionaryBuild()
    {
        if (weaponsStatusData != null)
        {
            weaponsStatusData = null;
            Debug.Log("SO_WeaponsBase: WeaponsStatusDictionary nullable");
        }

        weaponsStatusData = new Dictionary<int, bool>();
        for (short i = 0; i < weapons.Length; i++)
        {
            weaponsStatusData.Add(i, false);
        }
    }

    public void SetDefault()
    {
        weaponsStatusData = null;
        IsLoaded = false;
    }
}
