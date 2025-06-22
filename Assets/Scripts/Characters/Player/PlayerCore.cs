using UnityEngine;

public class PlayerCore : Character
{
    public SOPlayerData playerData;
    
    InputManager inputManager;

    

    //dynamic cast

    PlayerHealthSystem PlayerHealthSystem => HealthSystem as PlayerHealthSystem;

    PlayerPerception PlayerPerception => AIPerception as PlayerPerception;



    //is target not null

    public bool IsAiming => PlayerPerception?.AttackTarget != null;



    //init

    private void Awake()
    {
        inputManager = FindAnyObjectByType<InputManager>();

        if (inputManager == null) Debug.LogWarning("PLAYER_CONTROLLER :: health system is null");
    }



    //update

    private void Update()
    {
        MovementControl();
        UseItemControl();
        FlipControl();
        WeaponAimControl();
    }



    //helpers

    void MovementControl()
    {
        //disable when healing

        if (PlayerHealthSystem.IsHealing)
        {
            return;
        }


        //movement

        MovementSystem.Move(inputManager.MoveAxis, MovementSystem.RunSpeed);
        CharacterAnimationController.SetMoveAxis(inputManager.MoveAxis);
    }

    void UseItemControl()
    {
        //disable when healing

        if (PlayerHealthSystem.IsHealing)
        {
            return;
        }


        //use active item

        if (inputManager.AttackTrigger)
        {
            EquipmentSystem.ActiveItem?.Use();
        }
    }

    void FlipControl()
    {
        //disable when healing

        if (PlayerHealthSystem.IsHealing)
        {
            return;
        }


        //flip

        bool _autoFlip = !IsAiming;
        CharacterAnimationController.EnableAutoFlip(_autoFlip);

        if (!_autoFlip)
        {
            bool _flip = (PlayerPerception.AttackTarget.transform.position.x - transform.position.x) < 0.0f;
            CharacterAnimationController.Flip(_flip);
        }
    }

    void WeaponAimControl()
    {
        Weapon _weapon = EquipmentSystem.ActiveItem as Weapon;

        if (!PlayerHealthSystem.IsHealing && IsAiming)
        {
            _weapon?.Aim(PlayerPerception.AttackTarget.transform.position);
        }
        else
        {
            _weapon?.AimReset();
        }
    }
}
