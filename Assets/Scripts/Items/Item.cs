using UnityEngine;

public enum ItemType
{
    Common, Gun, Melee
}

public abstract class Item : MonoBehaviour, IUseable
{
    [SerializeField] ItemType itemType;
    [SerializeField] string itemName;
    [SerializeField] Sprite icon;
    [SerializeField] int price;
    [SerializeField] int requiredExpLevel;


    public string UserTag { get; set; }


    public ItemType ItemType
    {
        get => itemType;
    }

    public string ItemName
    {
        get => itemName;
    }

    public Sprite Icon
    {
        get
        {
            if (icon != null)
            {
                return icon;
            }

            SpriteRenderer spriteRenderer;

            if (TryGetComponent(out spriteRenderer))
            {
                return spriteRenderer.sprite;
            }
            else
            {
                return GetComponentInChildren<SpriteRenderer>()?.sprite;
            }
        }
    }

    public int Price
    {
        get => price;
    }

    public int RequiredExpLevel
    {
        get => requiredExpLevel;
    }


    public abstract void Use();
}
