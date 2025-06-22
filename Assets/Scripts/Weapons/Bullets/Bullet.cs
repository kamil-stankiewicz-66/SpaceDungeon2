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


    private void OnTriggerEnter2D(Collider2D collision) => CollisionCheck(collision);

    private void OnCollisionEnter2D(Collision2D collision) => CollisionCheck(collision.collider);


    void CollisionCheck(Collider2D collision)
    {
        //dont destroy if
        if (SkipCollision(collision))
        {
            return;
        }

        //hit
        IDamageable damageable;
        if (collision.TryGetComponent(out damageable))
        {
            damageable.Damage(bulletDamage);
        }
        else if (collision.transform.parent != null)
        {
            if (collision.TryGetComponent(out damageable))
            {
                damageable.Damage(bulletDamage);
            }
        }

        //log
        print("Bullet: destroyed " + collision.name + " " + collision.tag);

        //destroy
        Destroy(gameObject);
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
