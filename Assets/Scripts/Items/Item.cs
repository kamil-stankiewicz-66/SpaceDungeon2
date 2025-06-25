using UnityEngine;

public enum ItemType
{
    Common, Gun, Melee
}

public abstract class Item : MonoBehaviour, IUseable
{
    [SerializeField] int id;
    [SerializeField] ItemType itemType;
    [SerializeField] string itemName;
    [SerializeField] Sprite icon;
    [SerializeField] int price;
    [SerializeField] int requiredExpLevel;


    //getter

    public int Id { get => id; }

    public ItemType ItemType { get => itemType; }

    public string ItemName { get => itemName; }

    public Sprite Icon { get => icon; }

    public int Price { get => price; }

    public int RequiredExpLevel { get => requiredExpLevel; }


    //property

    public string UserTag { get; set; }


    //func

    public abstract void Use();

}
