using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] internal SOPlayerData playerData;
    [SerializeField] internal HealthSystemPlayer playerHealthSystem;
    [SerializeField] internal AttackInvokeSystem attackInvoke;
    [SerializeField] internal PlayerController playerController;
    [SerializeField] internal PlayerAttack playerAttack;
    [SerializeField] internal PlayerEnemieDetector playerEnemieDetector;
    [SerializeField] internal PlayerInput playerInput;

    [SerializeField] internal GameObject playerUI;
    [SerializeField] internal Animator moveAnim;
    [SerializeField] internal Transform playerBody;
    [SerializeField] internal Transform weaponHolder;
    [SerializeField] internal float speed;

    internal Transform player;

    internal bool isAiming => playerEnemieDetector.NearestEnemy != null;
    internal bool isFacingRight = true;


    /// <summary>
    /// private
    /// </summary>

    private void Awake()
    {
        player = transform;
    }

}
