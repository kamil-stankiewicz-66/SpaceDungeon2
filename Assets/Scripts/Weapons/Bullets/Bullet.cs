using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string bulletTag;
    public float bulletDamage;
    public float bulletSpeed;
    public GameObject particleEffect;

    Rigidbody2D rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision) => CollisionCheck(collision, false);

    private void OnCollisionEnter2D(Collision2D collision) => CollisionCheck(collision.collider, true);


    void CollisionCheck(Collider2D collision, bool destroy)
    {
        if (!SkipCollision(collision))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            Transform parent = collision.transform.parent;
            if (damageable == null && parent != null)
            {
                damageable = parent.GetComponent<IDamageable>();
            }

            if (damageable != null)
            {
                damageable.Damage(bulletDamage);
                destroy = true;
            }
        }

        if (destroy)
        {
            //log
            print("Bullet: destroyed " + collision.name + " " + collision.tag);

            //destroy
            Destroy(gameObject);
        }
    }

    bool SkipCollision(Collider2D collision)
    {
        return
            collision == null ||
            collision.CompareTag(bulletTag) ||
            collision.CompareTag(TAG.NOT_BULLET_COLLIDING);
    }


    private void OnDestroy()
    {
        if (GameMarks.ParticlesOn)
        {
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }
    }

}
