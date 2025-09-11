using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SOItemRegistry", menuName = "ScriptableObjects/DataObjects/SOItemRegistry")]
public class SOItemRegistry : ScriptableObject
{
    [SerializeField] SOItemData[] registry;

    public SOItemData[] Registry { get => registry; }


    public SOItemData Get(string id)
    {
        foreach (SOItemData item in registry)
        {
            if (item == null)
                continue;

            if (item.ID == id)
                return item;
        }

        return null;
    }

    public SOItemData[] GetByType(EItemType type)
    {
        return registry.Where(item => item.ItemType == type).ToArray();
    }
}
