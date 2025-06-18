using UnityEngine;

public enum WeaponType
{
    Gun
}

public abstract class Weapon : MonoBehaviour
{
    [HideInInspector] public string weaponTag; //set in weapon loader
    public WeaponType shopWeaponType;
    public string shopName;
    public Sprite shopIcon;
    public ushort shopPrice;
    public ushort shopRequiredExpLevel;

    public int attackTimeOut;
    public float range;
    public float damage;
    [SerializeField] protected Animator animator;

    public abstract void Attack();
}
