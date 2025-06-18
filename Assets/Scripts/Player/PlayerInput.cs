using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    InputAction moveAction;
    InputAction attackAction;
    InputAction healAction;

    Vector2 moveAxis;
    bool attack;
    bool heal;


    //getters

    public Vector2 MoveAxis
    {
        get => moveAxis;
    }

    public bool AttackTrigger
    {
        get => attack;
    }

    public bool HealTrigger
    {
        get => heal;
    }


    //enable and disable

    private void OnEnable() => inputActions.FindActionMap("Player").Enable();

    private void OnDisable() => inputActions.FindActionMap("Player").Disable();


    //init
    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        healAction = InputSystem.actions.FindAction("Heal");
    }


    //update

    private void Update()
    {
        moveAxis = moveAction.ReadValue<Vector2>();
        attack = attackAction.IsPressed();
        heal = healAction.IsPressed();
    }

}
