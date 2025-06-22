using UnityEngine;

public class Character : MonoBehaviour
{
    public MovementSystem MovementSystem { get; private set; }

    public CharacterAnimationController CharacterAnimationController { get; private set; }

    public HealthSystem HealthSystem { get; private set; }

    public EquipmentSystem EquipmentSystem { get; private set; }

    public AIPerception AIPerception { get; private set; }


    protected virtual void OnEnable()
    {
        MovementSystem = GetComponent<MovementSystem>();
        CharacterAnimationController = GetComponent<CharacterAnimationController>();
        HealthSystem = GetComponent<HealthSystem>();
        EquipmentSystem = GetComponent<EquipmentSystem>();
        AIPerception = GetComponent<AIPerception>();

        if (MovementSystem == null) Debug.LogWarning("CHARACTER :: movement system is null");
        if (CharacterAnimationController == null) Debug.LogWarning("CHARACTER :: animation controller is null");
        if (HealthSystem == null) Debug.LogWarning("CHARACTER :: input system is null");
        if (EquipmentSystem == null) Debug.LogWarning("CHARACTER :: equipment system is null");
        if (AIPerception == null) Debug.LogWarning("CHARACTER :: aiPerception is null");
    }
}

