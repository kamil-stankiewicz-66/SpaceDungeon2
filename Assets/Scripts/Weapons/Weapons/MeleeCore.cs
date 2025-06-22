using System.Collections.Generic;
using UnityEngine;

public class MeleeCore : Weapon
{
    //overrides

    public override void Aim(Vector2 target)
    {
        return;
    }

    protected override void AttackAction()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, Range, LAYER.TakeDamage);

        foreach (var enemy in enemiesInRange)
        {
            if (enemy.CompareTag(UserTag))
            {
                continue;
            }

            Transform parent = enemy.transform.parent;
            if (parent != null && parent.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(Damage);
            }

        }
    }

}
