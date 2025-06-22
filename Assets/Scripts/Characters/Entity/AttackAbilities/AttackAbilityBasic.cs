using UnityEngine;

public class AttackAbilityBasic : Ability
{
    Character character;
    Transform player;


    Weapon WeaponCore => character.EquipmentSystem.ActiveItem as Weapon;


    private void Awake()
    {
        character = GetComponent<Character>();
        player = FindAnyObjectByType<PlayerCore>()?.transform;
    }


    public override void Init() { }

    public override void Execute()
    {
        if (character.MovementSystem == null)
        {
            return;
        }

        if (character.EquipmentSystem == null)
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        if (WeaponCore == null)
        {
            return;
        }

        BasicAttack();
    }


    void BasicAttack()
    {
        if (WeaponCore.transform.DistanceTo(player) < WeaponCore.Range && player.gameObject.activeSelf)
        {
            character.CharacterAnimationController.EnableAutoFlip(false);
            character.CharacterAnimationController.Flip(player.position.x - WeaponCore.transform.position.x < 0.0f);

            WeaponCore.Aim(player.position);
            WeaponCore.Use();
        }
        else
        {
            character.CharacterAnimationController.EnableAutoFlip(true);
            Vector2 _direction = player.position - transform.position;
            character.MovementSystem.Move(_direction.normalized, character.MovementSystem.RunSpeed);
            WeaponCore.AimReset();
        }
    }

}
