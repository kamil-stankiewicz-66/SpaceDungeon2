using UnityEngine;

[CreateAssetMenu(fileName = "SOPlayerData", menuName = "ScriptableObjects/DataObjects/SOPlayerData")]
public class SOPlayerData : ScriptableObject
{
    [SerializeField] SOParameters so_parameters;

    int exp;
    int weaponID;
    int coins;


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
        get => 1 + Exp / so_parameters.PLAYER_EXP2PROMOTION;
    }

    public int Health
    {
        get => ExpLevel * so_parameters.PLAYER_HEALTH_BASE;
    }

    public int Damage
    {
        get => ExpLevel;
    }
}

public static class SOPlayerDataExtensions
{
    public static void AddExp(this SOPlayerData data, int exp)
    {
        data.Exp += exp;
    }
}