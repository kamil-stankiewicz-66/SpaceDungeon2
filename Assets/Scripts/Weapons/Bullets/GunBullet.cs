using UnityEngine;

public class GunBullet : Bullet
{
    [HideInInspector] public Vector3 direction;

    public override void OnTriggerEnter2D(Collider2D collision) => base.OnTriggerEnter2D(collision);

    private void OnEnable()
    {
        rb.linearVelocity = transform.right * bulletSpeed;
    }

}
