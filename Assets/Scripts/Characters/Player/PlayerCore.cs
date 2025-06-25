using UnityEngine;
using UnityEngine.InputSystem;

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


        //disable when healing

        if (PlayerHealthSystem.IsHealing)
        {
            _weapon?.AimReset();
            return;
        }


        //aim and flip

        if (IsAiming)
        {
            WeaponAimControllerHelper(_weapon, PlayerPerception.AttackTarget.transform.position);
        }
        else
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = 0f;

            WeaponAimControllerHelper(_weapon, mouseWorldPos);
        }
    }

    void WeaponAimControllerHelper(Weapon weapon, Vector3 target)
    {
        weapon?.Aim(target);

        bool _flip = (target.x - transform.position.x) < 0.0f;
        CharacterAnimationController.Flip(_flip);
    }
}
