using UnityEngine;
using UnityEngine.UI;

public abstract class HealthSystem : MonoBehaviour
{
    [SerializeField] protected Image healthSlider;

    private int health_max;
    [SerializeField] private float health;


    /// <summary>
    /// virtual get and set
    /// </summary>

    public virtual int Health_max
    {
        get => health_max;
        set
        {
            if (value < health)
                Health = value;

            health_max = value;
            SliderRefresh();
        }
    }

    public virtual float Health
    {
        get => health;
        set
        {
            if (value > health_max)
                health = health_max;



            health = value;
            SliderRefresh();
        }
    }


    /// <summary>
    /// virtual methods
    /// </summary>

    public virtual void Damage(float damage)
    {
        if (!GameMarks.DamageTakingOn)
            return;

        if (Health <= 0)
            return;

        Health -= damage;
    }

    public virtual void Heal(float value)
    {
        if (!GameMarks.HealTakingOn)
            return;

        if (Health + value > Health_max)
            Health = Health_max;
        else
            Health += value;
    }


    /// <summary>
    /// private methods
    /// </summary>

    protected void SliderRefresh()
    {
        healthSlider.RefreshSlider(Health, Health_max);
    }


}
