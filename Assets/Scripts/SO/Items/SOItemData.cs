using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Common, Gun, Melee
}

[CreateAssetMenu(fileName = "SOItemData", menuName = "ScriptableObjects/Items/SOItemData")]
public class SOItemData : ScriptableObject
{
    [SerializeField] string id;
    [SerializeField] ItemType itemType;
    [SerializeField] string itemName;
    [SerializeField] Sprite icon;
    [SerializeField] Item item;
    [SerializeField] int coinsValue;


    //getter

    public string ID { get => id; }

    public ItemType ItemType { get => itemType; }

    public string ItemName { get => itemName; }

    public Sprite Icon { get => icon; }

    public Item Item { get => item; }

    public GameObject Prefab { get => item.gameObject; }

    public int CoinsValue { get => coinsValue; }


    //validate

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
    }
#endif

}
