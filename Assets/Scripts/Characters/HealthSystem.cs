using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject particleEffect_blood;
    [SerializeField] GameObject body;

    [SerializeField] Image healthSlider;
    [SerializeField] float health;



    //enable

    private void Awake()
    {
        if (health > 0.0f)
        {
            Health = health;
        }
    }



    //disable

    protected void OnDisable()
    {
        if (!GameMarks.ParticlesOn)
        {
            print($"{gameObject.name}: HealthSystemEntity: blood particle effect skiped (particles disabled in game marks)");
            return;
        }

        if (particleEffect_blood != null)
        {
            Instantiate(particleEffect_blood, transform.position, Quaternion.identity);
        }
    }

    private void OnApplicationQuit() => GameMarks.ParticlesOn = false;



    // get and set

    public float HealthMax { get; private set; }

    public virtual float Health
    {
        get => health;
        set
        {           
            health = value;
            
            if (health > HealthMax)
            {
                HealthMax = health;
            }

            SliderRefresh();

            if (IsDead)
            {
                gameObject.SetActive(false);
            }
        }
    }


    public bool IsDead => health <= 0.0f;



    //methods

    public void Damage(float damage)
    {
        AddHealth(-damage);
    }

    public void AddHealth(float value)
    {
        if (!GameMarks.HealthChangeOn)
        {
            return;
        }

        float temp = Health + value;
        temp = Mathf.Clamp(temp, 0.0f, HealthMax);
        Health = temp;
    }



    //private methods

    private void SliderRefresh()
    {
        healthSlider.RefreshSlider(Health, HealthMax);
    }
}
