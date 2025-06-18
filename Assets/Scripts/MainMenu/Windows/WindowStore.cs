using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowStore : MonoBehaviour
{
    [SerializeField] SOWeaponsBase so_weaponsBase;
    [SerializeField] SOPlayerData so_playerData;
    [SerializeField] NotificationCaller notificationCaller;
    [SerializeField] DialogWindowCaller dialogWindowCaller;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] Transform weaponsIconsHolder;
    [SerializeField] GameObject weaponIconPrefab;

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

    private ItemStoreIcon[] icons;
    private ItemData selectedItem;


    /// <summary>
    /// public
    /// </summary>

    public void Buy()
    {
        if (selectedItem == null ||
            selectedItem.Status != ItemStatus.Buy)
        {
            notificationCaller.Show("ERROR");
            Reload();
            return;
        }

        if (so_playerData.Coins < selectedItem.Core.shopPrice)
        {
            notificationCaller.Show("You don't have enough coins.");
            return;
        }

        string _m = $"Are you sure you want to buy:\n'{selectedItem.Core.shopName}'\n" + $"for: {selectedItem.Core.shopPrice} coins?";
        UnityEvent _acceptEvent = new UnityEvent();
        _acceptEvent.AddListener(() =>
        {
            so_weaponsBase.SetWeaponStatus(selectedItem.Id, true);
            so_playerData.Coins -= selectedItem.Core.shopPrice;
            Reload();
        });

        dialogWindowCaller.Show(_m, _acceptEvent);
    }

    public void Use()
    {
        if (selectedItem == null ||
            selectedItem.Status != ItemStatus.Use)
        {
            notificationCaller.Show("ERROR");
            return;
        }

        if (so_playerData.WeaponID == selectedItem.Id)
        {
            return;
        }

        so_playerData.WeaponID = selectedItem.Id;
        Reload();
    }


    /// <summary>
    /// unity callers
    /// </summary>

    bool isIconsLoaded;
    private void OnEnable()
    {
        if (so_playerData.WeaponID > -1)
        {
            SelectItem(so_playerData.WeaponID);
        }
        else
        {
            SelectItem(0);
        }

        RefreshScrollbar();

        if (isIconsLoaded)
            return;

        icons = new ItemStoreIcon[so_weaponsBase.WeaponsCount];
        for (short i = 0; i < so_weaponsBase.WeaponsCount; i++)
        {
            Weapon _weapon = so_weaponsBase.GetWeaponCore(i);
            ItemStoreIcon _itemShopIcon =
                Instantiate(weaponIconPrefab, weaponsIconsHolder)
                .GetComponent<ItemStoreIcon>();

            icons[i] = _itemShopIcon;
            _itemShopIcon.Set(_weapon.shopIcon,
                                      _weapon.shopName,
                                      GetItemStatus(i, _weapon) != ItemStatus.Locked,
                                      i,
                                      (short s) => SelectItem(s));
        }
        RefreshIcons();
        RefreshScrollbar();
        isIconsLoaded = true;

    }

    private void OnDisable()
    {
        selectedItem = null;
        Debug.Log("WindowShop: selectedItem cleared");
    }


    /// <summary>
    /// private methods
    /// </summary>
    
    private void Reload()
    {
        SelectItem(selectedItem.Id);
    }

    private void SelectItem(int _id)
    {
        if (0 > _id || _id > so_weaponsBase.WeaponsCount)
        {
            Debug.LogWarning($"WindowShop: selectedItem is out of array: {_id}/{so_weaponsBase.WeaponsCount}");
            return;
        }

        Weapon _core = so_weaponsBase.GetWeaponCore(_id);
        ItemStatus _itemStatus = GetItemStatus(_id, _core);
        selectedItem = new ItemData(_id, _core, _itemStatus);
        Debug.Log($"WindowShop: selectedItem data created for id {_id}");

        RefreshIcons();
        RefreshStatsViewPanel();
    }

    private ItemStatus GetItemStatus(int _id, Weapon _core)
    {
        if (_core == null)
            return default;

        if (_core.shopRequiredExpLevel > so_playerData.ExpLevel)
            return ItemStatus.Locked;

        if (!so_weaponsBase.GetWeaponStatus(_id))
            return ItemStatus.Buy;

        if (_id != so_playerData.WeaponID)
            return ItemStatus.Use;

        else
            return ItemStatus.Using;
    }

    private void RefreshIcons()
    {
        if (icons == null)
            return;

        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].ChangeState(i == selectedItem.Id);
        }
    }

    private async void RefreshScrollbar()
    {
        int _max = so_weaponsBase.WeaponsCount - 1;
        int _i = _max - selectedItem.Id;
        float _v = (float)_i / (float)_max;
        _v = Mathf.Clamp01(_v);

        await Task.Delay(10);
        scrollbar.value = _v;
    }

    private void RefreshStatsViewPanel()
    {
        if (selectedItem == null)
        {
            Debug.LogWarning("WindowShop: RefreshStatsViewPanel(): selected item is NULL");
            return;
        }

        switch (selectedItem.Status)
        {
            case ItemStatus.Locked: ChangeBar(lockedBar); break;
            case ItemStatus.Use: ChangeBar(useBar); break;
            case ItemStatus.Using: ChangeBar(usingBar); break;
            case ItemStatus.Buy: ChangeBar(buyBar); break;
        }

        text_coins.text = so_playerData.Coins.ToString();
        text_weapon_price.text = $"{selectedItem.Core.shopPrice} coins";
        text_weapon_levelRequired.text = $"Required level: {selectedItem.Core.shopRequiredExpLevel}";

        if (selectedItem.Status == ItemStatus.Locked)
        {
            string _locked = "???";
            text_weapon_name.text = _locked;
            text_weapon_weaponType.text = _locked;
            text_weapon_damage.text = _locked;
            text_weapon_fireRate.text = _locked;
        }
        else
        {
            text_weapon_name.text = selectedItem.Core.shopName;
            text_weapon_weaponType.text = selectedItem.Core.shopWeaponType.ToString();
            text_weapon_damage.text = selectedItem.Core.damage.ToString();
            text_weapon_fireRate.text = (1000.0f / selectedItem.Core.attackTimeOut).RoundTo(1).ToString() + "/s";
        }

        Debug.Log("WindowShop: StatsViewPanel refreshed.");
    }

    private void ChangeBar(GameObject _bar)
    {
        lockedBar.SetActive(_bar == lockedBar);
        buyBar.SetActive(_bar == buyBar);
        useBar.SetActive(_bar == useBar);
        usingBar.SetActive(_bar == usingBar);
    }


    /// <summary>
    /// private objects
    /// </summary>

    private enum ItemStatus { Locked, Use, Using, Buy }

    private class ItemData
    {
        private int _id;
        private Weapon _core;
        private ItemStatus _status;

        public ItemData(int id, Weapon core, ItemStatus itemStatus)
        {
            _id = id;
            _core = core;
            _status = itemStatus;
        }
        
        public int Id => _id;

        public Weapon Core => _core;

        public ItemStatus Status => _status;
    }



}
