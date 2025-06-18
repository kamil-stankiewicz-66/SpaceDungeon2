using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    public string bulletTag;
    public float bulletDamage;
    public float bulletSpeed;
    public GameObject particleEffect;


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //dont destroy if:
        if (collision.CompareTag(bulletTag) || 
            collision.CompareTag(TAG.NOT_BULLET_COLLIDING) ||
            collision.isTrigger)
        {
            return;
        }

        //hit
        if (collision.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.Damage(bulletDamage);
        }

        print("Bullet: destroyed " + collision.name + " " + collision.tag);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(particleEffect, transform.position, Quaternion.identity);
    }

}
