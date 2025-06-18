using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //core
    [SerializeField] PlayerCore playerCore;


    private Weapon _currentWeaponCore;
    private GameObject _currentWeapon;
    public GameObject CurrentWeapon
    {
        set
        {
            _currentWeapon = value;
           
            WeaponLoader.LoadWeaponData(playerCore.weaponHolder,
                                        _currentWeapon,
                                        out _currentWeaponCore);

            _currentWeaponCore.damage += playerCore.playerData.Damage;

        }

        get => _currentWeapon;
    }
    

    private void Update()
    {
        //aim
        playerCore.playerBody.Aim(
            ref playerCore.weaponHolder, 
            playerCore.playerEnemieDetector.NearestEnemy, 
            ref playerCore.isFacingRight);

        //main attack
        if (playerCore.playerInput.AttackTrigger)
        {
            PlayerMainAttack();
        }
    }


    private void PlayerMainAttack()
    {
        if (_currentWeaponCore == null)
            return;

        playerCore.attackInvoke?.AttackInvoke(_currentWeaponCore);

    }


}
