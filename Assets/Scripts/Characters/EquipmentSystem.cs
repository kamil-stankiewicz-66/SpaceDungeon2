using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemEntry
{
    public Item item;
    public uint amount;
}

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] bool autoLoot;
    [SerializeField] Item activeItem;
    [SerializeField] List<ItemEntry> equipment;


    public Item ActiveItem
    {
        get => activeItem;
        private set => activeItem = value;
    }



    //add and remove item

    public void AddItemToEquipment(Item item, uint amount = 1u)
    {
        if (amount == 0)
        {
            return;
        }


        int index = equipment.FindIndex(e => e.item == item);

        if (index < 0)
        {
            equipment.Add(new ItemEntry { item = item, amount = amount });
            return;
        }


        ItemEntry updated = equipment[index];
        updated.amount += amount;
        equipment[index] = updated;
    }

    public void RemoveItemFromEquipment(Item item, uint amount = 1u)
    {
        if (amount == 0)
        {
            return;
        }


        int index = equipment.FindIndex(e => e.item == item);

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
        AddItemToEquipment(ActiveItem);
        ActiveItem = null;
    }

    public void SetActiveItemFromEquipment(Item item)
    {
        RemoveItemFromEquipment(item);
        SetActiveItem(item);
    }

    public void SetActiveItemFromEquipment(int index)
    {
        ItemEntry itemEntry = equipment.Get(index);
        SetActiveItemFromEquipment(itemEntry.item);
    }



    //engnine

    private void OnEnable()
    {
        SetActiveItem(ActiveItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!autoLoot)
        {
            return;
        }

        if (collision == null)
        {
            return;
        }

        if (collision.TryGetComponent(out Item item))
        {
            AddItemToEquipment(item);
        }
    }

}
