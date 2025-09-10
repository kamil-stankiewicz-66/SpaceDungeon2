using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowItemStore : MonoBehaviour
{
    private enum ItemStatus { Locked, Use, Using, Buy }

    [SerializeField] SOItemRegistry SOItemRegistry;
    [SerializeField] SOPlayerData SOPlayerData;
    [SerializeField] NotificationCaller notificationCaller;
    [SerializeField] DialogWindowCaller dialogWindowCaller;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] Transform iconsHolder;
    [SerializeField] GameObject iconPrefab;

    [SerializeField]
    TextMeshProUGUI
        text_coins,
        text_weapon_name,
        text_weapon_weaponType,
        text_weapon_damage,
        text_weapon_fireRate,
        text_weapon_price,
        text_weapon_levelRequired;

    [SerializeField]
    GameObject
        lockedBar,
        buyBar,
        useBar,
        usingBar;

    SOItemData m_selectedItem;



    //engine

    private void OnEnable()
    {
        //load all weapons
        SOItemData[] weapons = SOItemRegistry.Registry.Where(item => item.Item as Weapon != null).ToArray();


        //deafault selection
        m_selectedItem = weapons.FirstOrDefault(item => item.ID == SOPlayerData.ActiveItem);
        if (m_selectedItem == null)
        {
            m_selectedItem = weapons.Get(0);
        }


        //load icons
        iconsHolder.DestroyAllChilds();
        CreateIcons(weapons);


        //refresh all
        RefreshAll();
    }



    //refresh

    void RefreshAll()
    {
        RefreshViewPanel();
        RefreshIcons();
    }



    //item status

    ItemStatus GetItemStatus(SOItemData data)
    {
        if (data == null)
            return ItemStatus.Locked;

        if (data.ID == SOPlayerData.ActiveItem)
            return ItemStatus.Using;

        if (SOPlayerData.Equipment.Contains(data.ID))
            return ItemStatus.Use;

        Weapon weapon = data.Item as Weapon;
        if (weapon != null && weapon.RequiredExpLevel > SOPlayerData.ExpLevel)
        {
            return ItemStatus.Locked;
        }

        return ItemStatus.Buy;
    }



    //icons in scrollbar

    void CreateIcons(SOItemData[] items)
    {
        foreach (SOItemData item in items)
        {
            ItemStoreIcon icon = Instantiate(iconPrefab, iconsHolder).GetComponent<ItemStoreIcon>();
            
            icon.Set(item.Icon,
                     item.ItemName,
                     GetItemStatus(item) != ItemStatus.Locked,
                     item,
                     (SOItemData s) => SelectItem(s));
        }
    }

    private void RefreshIcons()
    {
        ItemStoreIcon[] icons = iconsHolder.GetComponentsInChildren<ItemStoreIcon>();

        foreach (ItemStoreIcon icon in icons)
        {
            if (icon == null)
                continue;

            SOItemData itemData = icon.ItemData;
            icon.ChangeState(itemData == m_selectedItem);
        }
    }



    //stats panel

    void RefreshViewPanel()
    {
        if (m_selectedItem == null)
        {
            Debug.LogWarning("WINDOW_STORE :: selected item is NULL");
            return;
        }

        ItemStatus _itemStatus = GetItemStatus(m_selectedItem);

        switch (_itemStatus)
        {
            case ItemStatus.Locked: ChangeBar(lockedBar); break;
            case ItemStatus.Use: ChangeBar(useBar); break;
            case ItemStatus.Using: ChangeBar(usingBar); break;
            case ItemStatus.Buy: ChangeBar(buyBar); break;
        }

        text_coins.text = SOPlayerData.Coins.ToString();
        text_weapon_price.text = $"{m_selectedItem.CoinsValue} coins";

        Weapon _core = m_selectedItem.Item as Weapon;
        string _rlvl = _core == null ? "0" : _core.RequiredExpLevel.ToString();
        text_weapon_levelRequired.text = $"Required level: {_rlvl}";

        if (_itemStatus == ItemStatus.Locked)
        {
            string _locked = "???";
            text_weapon_name.text = _locked;
            text_weapon_weaponType.text = _locked;
            text_weapon_damage.text = _locked;
            text_weapon_fireRate.text = _locked;
        }
        else
        {
            text_weapon_name.text = m_selectedItem.ItemName;
            text_weapon_weaponType.text = m_selectedItem.ItemType.ToString();
            text_weapon_damage.text = _core.Damage.ToString();
            text_weapon_fireRate.text = (1000.0f / _core.AttackTimeOut).RoundTo(1).ToString() + "/s";
        }

        Debug.Log("WINDOW_STORE :: StatsViewPanel refreshed.");
    }

    void ChangeBar(GameObject _bar)
    {
        lockedBar.SetActive(_bar == lockedBar);
        buyBar.SetActive(_bar == buyBar);
        useBar.SetActive(_bar == useBar);
        usingBar.SetActive(_bar == usingBar);
    }



    //actions

    void SelectItem(SOItemData item)
    {
        m_selectedItem = item;

        RefreshAll();
    }

    public void BuyItem()
    {
        if (GetItemStatus(m_selectedItem) != ItemStatus.Buy)
        {
            notificationCaller.Show("ERROR");
            RefreshAll();
            return;
        }

        if (SOPlayerData.Coins < m_selectedItem.CoinsValue)
        {
            notificationCaller.Show("You don't have enough coins.");
            return;
        }

        string _m = $"Are you sure you want to buy:\n'{m_selectedItem.ItemName}'\n" + $"for: {m_selectedItem.CoinsValue} coins?";
        UnityEvent _acceptEvent = new UnityEvent();
        _acceptEvent.AddListener(() =>
        {
            SOPlayerData.AddCoins(-m_selectedItem.CoinsValue);
            SOPlayerData.Equipment.Add(m_selectedItem.ID);
            RefreshAll();
        });

        dialogWindowCaller.Show(_m, _acceptEvent);
    }

    public void Use()
    {
        if (GetItemStatus(m_selectedItem) != ItemStatus.Use)
        {
            notificationCaller.Show("ERROR");
            return;
        }

        if (SOPlayerData.ActiveItem == m_selectedItem.ID)
        {
            return;
        }

        SOPlayerData.SetActiveItem(m_selectedItem.ID);
        RefreshAll();
    }
}
