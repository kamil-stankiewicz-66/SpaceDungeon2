using UnityEngine;
using UnityEngine.UI;

public class HealthSystemPlayer : HealthSystem
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] LevelFinalScreen levelFinalScreen;
    [SerializeField] GameObject particleEffect_blood;
    [SerializeField] Image healStaminaSlider;

    [SerializeField] private float healStamina;
    [SerializeField] private float healStamina_max;
    [SerializeField] private float healStamina_speedRegeneration;
    [SerializeField] private float healStamina_speedRegeneration_inFightBonus;
    [SerializeField] private float healStamina_speedHealing;
    [SerializeField] private float healStamina_speedConsumption;

    /// <summary>
    /// get and set
    /// </summary>

    public float HealStamina
    {
        get => healStamina;
        set
        {
            if (value > healStamina_max)
                healStamina = healStamina_max;
            else
                healStamina = value;

            healStaminaSlider.RefreshSlider(healStamina, healStamina_max);
        }
    }

    public float HealStamina_max
    {
        get => healStamina;
        set
        {
            healStamina_max = value;

            if (healStamina > healStamina_max)
                healStamina = healStamina_max;

            healStaminaSlider.RefreshSlider(healStamina, healStamina_max);
        }
    }


    /// <summary>
    /// private get
    /// </summary>

    private float HealStaminaByFrame
    {
        get => healStamina_max * healStamina_speedRegeneration * Time.fixedDeltaTime * 0.33f;
    }


    /// <summary>
    /// overrides
    /// </summary>
   
    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;

            if (value <= 0)
            {
                playerCore.playerBody.gameObject.SetActive(false);

                if (particleEffect_blood != null && GameMarks.ParticlesOn)
                    Instantiate(particleEffect_blood, transform.position, Quaternion.identity);

                levelFinalScreen.ShowWindow(LevelFinalScreen.FinalScreenModeEnum.DeadScreen);
            }
        }
    }


    /// <summary>
    /// private
    /// </summary>

    private void Update()
    {
        //heal or collect stamina
        if (playerCore.playerInput.HealTrigger)
        {
            HealByHealStamina();
        }
        else if (GameMarks.HealStaminaTakingOn)
        {
            HealStamina += playerCore.playerEnemieDetector.NearestEnemy != null 
                ? HealStaminaByFrame * healStamina_speedRegeneration_inFightBonus 
                : HealStaminaByFrame;
        }
    }

    private void HealByHealStamina()
    {
        Heal(HealStaminaByFrame * healStamina_speedHealing);
        HealStamina -= HealStaminaByFrame * healStamina_speedConsumption;
    }


}
