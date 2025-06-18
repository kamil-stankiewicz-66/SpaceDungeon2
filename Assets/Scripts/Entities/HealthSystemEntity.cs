using UnityEngine;

public class HealthSystemEntity : HealthSystem
{
    [SerializeField] GameObject particleEffect_blood;


    /// <summary>
    /// overrides
    /// </summary>

    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;

            if (base.Health <= 0)
                gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// private functions
    /// </summary>

    private void OnApplicationQuit() => GameMarks.ParticlesOn = false;

    protected void OnDisable()
    {
        if (!GameMarks.ParticlesOn)
        {
            print($"{gameObject.name}: HealthSystemEntity: blood particle effect skiped (particles disabled in game marks)");
            return;
        }

        if (particleEffect_blood != null)
            Instantiate(particleEffect_blood, transform.position, Quaternion.identity);
    }

}
