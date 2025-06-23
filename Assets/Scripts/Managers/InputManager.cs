using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    InputAction moveAction;
    InputAction attackAction;
    InputAction healAction;
    InputAction pauseAction;


    //getters

    public Vector2 MoveAxis { get; private set; }

    public bool AttackTrigger { get; private set; }

    public bool HealTrigger { get; private set; }

    public bool PauseTrigger { get; private set; }


    //enable and disable

    private void OnEnable() => inputActions.FindActionMap("Player").Enable();

    private void OnDisable() => inputActions.FindActionMap("Player").Disable();


    //init

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        healAction = InputSystem.actions.FindAction("Heal");
        pauseAction = InputSystem.actions.FindAction("Pause");
    }


    //update

    private void Update()
    {
        MoveAxis = moveAction.ReadValue<Vector2>();
        AttackTrigger = attackAction.IsPressed();
        HealTrigger = healAction.IsPressed();
        PauseTrigger = pauseAction.IsPressed();
    }

}
