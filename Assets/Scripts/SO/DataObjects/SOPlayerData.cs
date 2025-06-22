using UnityEngine;

[CreateAssetMenu(fileName = "SOPlayerData", menuName = "ScriptableObjects/DataObjects/SOPlayerData")]
public class SOPlayerData : ScriptableObject
{
    [SerializeField] SOParameters so_parameters;

    [SerializeField] int exp;
    [SerializeField] int weaponID;
    [SerializeField] int coins;


    public bool IsLoaded { get; set; }


    //get and set

    public int Exp
    {
        get => exp;
        set => exp = value;
    }

    public int WeaponID
    {
        get => weaponID;
        set => weaponID = value;
    }

    public int Coins
    {
        get => coins;
        set => coins = value;
    }


    //only get

    public int ExpLevel
    {
        get => 1 + (Exp / so_parameters.PLAYER_EXP2PROMOTION);
    }

    public int Health
    {
        get => ExpLevel * so_parameters.PLAYER_HEALTH_BASE;
    }

    public int Damage
    {
        get => ExpLevel;
    }


    //set default

    public void SetDefault()
    {
        exp = 0;
        weaponID = -1;
        coins = 0;
        IsLoaded = false;
    }
}

public static class SOPlayerDataExtensions
{
    public static void AddExp(this SOPlayerData data, int exp)
    {
        data.Exp += exp;
    }
}