using UnityEngine;

public class HandsCore : Weapon
{
    [SerializeField] string enemyTag;
    Collider2D[] enemiesInRange;


    /// <summary>
    /// overrides
    /// </summary>

    public override void Attack()
    {
        enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (var enemy in enemiesInRange)
        {
            if (!enemy.CompareTag(enemyTag))
                continue;

            if (enemy.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.Damage(damage);
                print($"{gameObject.name}: HandsCore: Damaged enemy with tag: " + enemyTag);
            }
        }
    }


    /// <summary>
    /// private methods
    /// </summary>

    private void OnEnable()
    {
        enemyTag = transform.parent.tag == TAG.PLAYER ? TAG.ENEMY : TAG.PLAYER;
    }
    

}
