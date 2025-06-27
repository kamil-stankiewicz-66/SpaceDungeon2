using UnityEngine;

public class GunCore : Weapon
{
    [SerializeField] Transform gunBarrel;
    [SerializeField] GameObject bulletPrefab;


    //init
    protected override void OnEnable()
    {
        base.OnEnable();

        float t = animator.AnimationClipTime(ANIM) * 1000;
        if (AttackTimeOut < t)
        {
            float x = t / AttackTimeOut;
            animator.speed = x;

            print($"{gameObject.name} GunCore: anim lenght: " + t);
            print($"{gameObject.name} GunCore: w lenght: " + AttackTimeOut);
            print($"{gameObject.name} GunCore: set animator speed to: " + x);
        }
    }


    //aim
    public override void Aim(Vector2 target)
    {
        Vector2 position = transform.position;
        Vector2 direction = target - position;

        float aimAngle = 0.0f;

        if (direction != Vector2.zero)
        {
            direction.Normalize();
            aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        bool isFliped = transform.lossyScale.x < 0.0f;
        if (isFliped)
        {
            aimAngle += 180.0f;
        }

        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }


    //attack
    protected override void AttackAction()
    {
        Transform parent = transform.parent;

        Quaternion aimAngle = transform.rotation;
        if (transform.lossyScale.x < 0f)
        {
            aimAngle *= Quaternion.Euler(0f, 0f, 180f);
        }

        GameObject _newBullet = Instantiate(bulletPrefab, gunBarrel.position, aimAngle);

        Bullet bullet = _newBullet.GetComponent<Bullet>();
        bullet.bulletTag = parent.tag;
        bullet.bulletDamage = Damage;
    }

}
