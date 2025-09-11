using UnityEditor;
using UnityEngine;

public enum EItemType
{
    Common,
    Gun, Melee,
    Key
}

[CreateAssetMenu(fileName = "SOItemData", menuName = "ScriptableObjects/Items/SOItemData")]
public class SOItemData : ScriptableObject
{
    [SerializeField] string id;
    [SerializeField] EItemType itemType;
    [SerializeField] string itemName;
    [SerializeField] Sprite icon;
    [SerializeField] Item core;
    [SerializeField] int coinsValue;


    //getter

    public string ID { get => id; }

    public EItemType ItemType { get => itemType; }

    public string ItemName { get => itemName; }

    public Sprite Icon { get => icon; }

    public Item Core { get => core; }

    public GameObject Prefab { get => core.gameObject; }

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
