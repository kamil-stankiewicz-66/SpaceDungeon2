using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : HealthSystem
{
    [SerializeField] Image healPointsSlider;

    [SerializeField] float healPoints_regnerationRate;
    [SerializeField] float healPoints_healingRate;
    
    InputManager inputManager;

    float healPoints;



    //flag

    public bool IsHealing { get; private set; }



    //get and set

    public float HealPointsMax { get; private set; }

    public float HealPoints
    {
        get => healPoints;
        set
        {
            healPoints = value;

            if (healPoints > HealPointsMax)
            {
                HealPointsMax = healPoints;
            }
            else if (healPoints < 0.0f)
            {
                healPoints = 0.0f;
            }

            RefreshHealSlider();
        }
    }



    //methods

    public void AddHealPoints(float value)
    {
        if (!GameMarks.HealPointsChangeOn)
        {
            return;
        }

        float temp = HealPoints + value;
        temp = Mathf.Clamp(temp, 0.0f, HealPointsMax);
        HealPoints = temp;
    }



    //private

    private void Awake()
    {
        inputManager = FindAnyObjectByType<InputManager>();
    }

    private void Update()
    {
        bool _canHeal =
            Health < HealthMax && 
            HealPoints > 0.0f;
        
        IsHealing = _canHeal && inputManager.HealTrigger;

        if (IsHealing)
        {
            float temp = HealPointsRate() * healPoints_healingRate;
            AddHealPoints(-temp);
            AddHealth(temp);
        }
        else
        {
            AddHealPoints(HealPointsRate() * healPoints_regnerationRate);
        }
    }




    //helpers

    void RefreshHealSlider()
    {
        healPointsSlider.RefreshSlider(HealPoints, HealPointsMax);
    }

    float HealPointsRate()
    {
        return HealPointsMax * Time.deltaTime;
    }
}
