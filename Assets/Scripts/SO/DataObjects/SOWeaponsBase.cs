using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOWeaponBase", menuName = "ScriptableObjects/DataObjects/WeaponsBase")]
public class SOWeaponsBase : ScriptableObject 
{
    [HideInInspector] public Dictionary<int, bool> weaponsStatusData;
    [SerializeField] private GameObject[] weapons;

    //block data
    public bool isLoaded = false;


    /// <summary>
    /// only get
    /// </summary>
    
    public int WeaponsCount
    {
        get => weapons.Length;
    }

    
    /// <summary>
    /// public methods
    /// </summary>

    public GameObject GetWeaponPrefab(int weaponNumber)
    {
        if (weaponNumber >= weapons.Length)
            return null;

        return weapons[weaponNumber];
    }

    public Weapon GetWeaponCore(int weaponID)
    {
        if (weaponID >= weapons.Length)
        {
            Debug.LogWarning($"SO_WeapondBase: id >= weapons.Length {weaponID} {weapons.Length}");
            return null;
        }

        Weapon _w = weapons[weaponID].GetComponentEx<Weapon>();

        if (_w == null)
        {
            Debug.LogWarning("SO_WeapondBase: Downloaded weapon is null.");
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
        weaponsStatusData.Remove(_id);
        weaponsStatusData.Add(_id, true);
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

    public void DataSetDefault()
    {
        weaponsStatusData = null;
        isLoaded = false;
    }
}
