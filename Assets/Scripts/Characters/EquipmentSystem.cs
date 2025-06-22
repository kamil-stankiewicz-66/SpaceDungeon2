using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] Item activeItem;


    public Item ActiveItem 
    { 
        get => activeItem; 
        private set => activeItem = value; 
    }


    private void OnEnable()
    {
        SetActiveItem(activeItem);
    }


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


        //create object
        ActiveItem = Instantiate(itemObj, hand.transform).GetComponent<Item>();
        ActiveItem.UserTag = gameObject.tag;

        Vector2 localScale = ActiveItem.transform.localScale;
        Vector3 parentScale = hand.transform.lossyScale;
        ActiveItem.transform.localScale = localScale / parentScale;
    }
}
