using UnityEngine;

[CreateAssetMenu(fileName = "SOEntityData", menuName = "ScriptableObjects/Entity/SOEntityData")]
public class SOEntityData : ScriptableObject
{
    [SerializeField] Sprite sprite;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;


    public Sprite Sprite
    {
        get => sprite;
    }

    public float WalkSpeed
    {
        get => walkSpeed;
    }

    public float RunSpeed
    {
        get => runSpeed;
    }
}
