using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOPlayerData", menuName = "ScriptableObjects/DataObjects/SOPlayerData")]
public class SOPlayerData : ScriptableObject
{
    [SerializeField] SOParameters so_parameters;

    [SerializeField] int exp;
    [SerializeField] int coins;
    [SerializeField] string activeItem;
    [SerializeField] List<string> equipment;


    public bool IsLoaded { get; set; }


    //only get

    public int Exp
    {
        get => exp;
    }

    public int Coins
    {
        get => coins;
    }

    public string ActiveItem
    {
        get => activeItem;
    }

    public List<string> Equipment
    {
        get => equipment;
    }

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


    //funcs

    public void AddExp(int exp)
    {
        this.exp += exp;
    }

    public void AddCoins(int value) => SetCoins(this.coins += value);

    public void SetCoins(int value)
    {
        this.coins = value;

        if (this.coins < 0)
        {
            this.coins = 0;
        }
    }

    public void SetActiveItem(string id)
    {
        this.activeItem = id;
    }

    public void SetEquipment(List<string> ids)
    {
        this.equipment = ids;
    }


    //load and save

    public void Load()
    {
        if (IsLoaded)
        {
            return;
        }

        if (Serializer.LoadBin(PATH.GetDirectory(PATH.PLAYERDATA_FILE), out Struct_PlayerData data))
        {
            activeItem = data.activeItem;
            equipment = data.equipment;
            coins = data.coins;
            exp = data.exp;

            Debug.Log("SOPlayerData: Player data loaded.");
        }
        else
        {
            exp = 0;
            activeItem = "";
            equipment.Clear();
            coins = 0;

            Debug.Log("SOPlayerData: Player data set default");
        }

        IsLoaded = true;
    }

    public void Save()
    {
        Struct_PlayerData dataCopy = new Struct_PlayerData()
        {
            activeItem = ActiveItem,
            equipment = Equipment,
            coins = Coins,
            exp = Exp
        };

        dataCopy.SaveBin(PATH.GetDirectory(PATH.PLAYERDATA_FILE));

        Debug.Log("SOPlayerData: PlayerData saved.");
    }

}