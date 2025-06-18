using UnityEngine;

public class GunCore : Weapon
{
    [SerializeField] Transform gunBarrel;
    [SerializeField] GameObject bulletPrefab;

    //anim
    const string SHOOT_ANIM = "Shoot";

    //attack
    public override void Attack()
    {
        Transform parent = transform.parent;
        Quaternion weaponQ = parent.rotation;
        GameObject _newBullet = Instantiate(bulletPrefab, gunBarrel.position, weaponQ);

        GunBullet bullet = _newBullet.GetComponent<GunBullet>();
        bullet.bulletTag = parent.tag;
        bullet.gameObject.tag = parent.tag;
        bullet.bulletDamage = damage;
        bullet.direction = weaponQ.ToAxisVector();

        animator.Play(SHOOT_ANIM);
    }

    private void OnEnable()
    {
        float t = animator.AnimationClipTime(SHOOT_ANIM) *1000;
        if (attackTimeOut < t)
        {
            float x = t / attackTimeOut;
            animator.speed = x;

            print($"{gameObject.name} GunCore: anim lenght: " + t);
            print($"{gameObject.name} GunCore: w lenght: " + attackTimeOut);
            print($"{gameObject.name} GunCore: set animator speed to: " + x);
        }
    }

}
