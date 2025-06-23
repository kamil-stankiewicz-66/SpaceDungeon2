using UnityEngine;

[System.Serializable]
public struct Struct_PlayerData
{
    public int weaponNr;
    public int coins;
    public int exp;
}

public class LS_PlayerData : MonoBehaviour
{
    [SerializeField] SOPlayerData SO_playerData;

    private void Awake()
    {
        if (SO_playerData.IsLoaded)
        {
            return;
        }

        print("LS_PlayerData: PlayerData loading started.");

        print("LS_PlayerData: PlayerData: weapon (not_loaded) " + SO_playerData.WeaponID);
        print("LS_PlayerData: PlayerData: exp_points (not_loaded) " + SO_playerData.Exp);
        print("LS_PlayerData: PlayerData: coins (not_loaded) " + SO_playerData.Coins);

        if (Serializer.LoadBin(PATH.GetDirectory(PATH.PLAYERDATA_FILE), out Struct_PlayerData data))
        {
            SO_playerData.WeaponID = data.weaponNr;
            SO_playerData.Coins = data.coins;
            SO_playerData.Exp = data.exp;

            print("LS_PlayerData: Player data loaded.");
        }
        else
        {
            SO_playerData.SetDefault();
        }

        print("LS_PlayerData: PlayerData: weapon (loaded) " + SO_playerData.WeaponID);
        print("LS_PlayerData: PlayerData: exp_points (loaded) " + SO_playerData.Exp);
        print("LS_PlayerData: PlayerData: coins (loaded) " + SO_playerData.Coins);

        SO_playerData.IsLoaded = true;
    }

    private void OnDestroy()
    {
        Struct_PlayerData dataCopy = new Struct_PlayerData()
        {
            weaponNr = SO_playerData.WeaponID,
            coins = SO_playerData.Coins,
            exp = SO_playerData.Exp
        };

        dataCopy.SaveBin(PATH.GetDirectory(PATH.PLAYERDATA_FILE));
        print("LS_PlayerData: PlayerData saved.");
    }

}
