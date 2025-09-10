using UnityEngine;

public abstract class Item : MonoBehaviour, IUseable
{
    [SerializeField] SOItemData data;

    public SOItemData Data { get => data; }


    //property

    public string UserTag { get; set; }


    //func

    public abstract void Use();

}
