using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemEntry
{
    public SOItemData item;
    public uint amount;
}

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] Item activeItem;
    [SerializeField] List<ItemEntry> equipment;
    [SerializeField] int coins;


    public Item ActiveItem
    {
        get => activeItem;
        private set => activeItem = value;
    }

    public List<string> GetIDList()
    {
        var temp = new List<string>();
        foreach (ItemEntry item in equipment)
        {
            for (int i = 0; i < item.amount; i++)
                temp.Add(item.item.ID);
        }

        return temp;
    }

    public int Coins { get => coins;}

    public void AddCoins(int value) => SetCoins(this.coins += value);

    public void SetCoins(int value)
    {
        this.coins = value;

        if (this.coins < 0)
        {
            this.coins = 0;
        }
    }



    //add and remove item

    public void AddItemToEquipment(SOItemData item, uint amount = 1u)
    {
        if (amount == 0)
        {
            return;
        }


        int index = equipment.FindIndex(e => e.item.ID == item.ID);

        if (index < 0)
        {
            equipment.Add(new ItemEntry { item = item, amount = amount });
            return;
        }


        ItemEntry updated = equipment[index];
        updated.amount += amount;
        equipment[index] = updated;
    }

    public void RemoveItemFromEquipment(SOItemData item, uint amount = 1u)
    {
        if (amount == 0)
        {
            return;
        }


        int index = equipment.FindIndex(e => e.item.ID == item.ID);

        if (index < 0)
        {
            return;
        }


        ItemEntry entry = equipment[index];
        entry.amount -= amount;
        
        if (entry.amount < 0)
        {
            equipment.RemoveAt(index);
        }
    }


    
    //items managment

    public void SetActiveItem(Item item)
    {
        if (item == null)
        {
            return;
        }

        if (item == ActiveItem && hand.transform.childCount > 0)
        {
            return;
        }

        if (hand == null)
        {
            return;
        }


        GameObject itemObj = item.gameObject;

        if (itemObj == null)
        {
            Debug.LogWarning("CHARACTER :: SET_ITEM_IN_HAND :: item dont have gameobject");
            return;
        }


        //hide to equipment
        HideActiveItem();

        //create object
        ActiveItem = Instantiate(itemObj, hand.transform).GetComponent<Item>();
        ActiveItem.UserTag = gameObject.tag;

        //fix scale
        Vector2 localScale = ActiveItem.transform.localScale;
        Vector3 parentScale = hand.transform.lossyScale;
        ActiveItem.transform.localScale = localScale / parentScale;
    }

    public void HideActiveItem()
    {
        //AddItemToEquipment(ActiveItem);
        ActiveItem = null;
    }

    public bool IsItemInEquipment(SOItemData item)
    {
        if (item == null)
            return false;

        foreach (var slot in equipment)
        {
            if (slot.item.ID == item.ID)
                return true;
        }

        return false;
    }



    //engnine

    private void OnEnable()
    {
        SetActiveItem(ActiveItem);
    }

}
