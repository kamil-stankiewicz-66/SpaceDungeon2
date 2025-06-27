using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    InputAction moveAction;
    InputAction analogAimAction;
    InputAction attackAction;
    InputAction healAction;
    InputAction pauseAction;


    //getters

    public Vector2 MoveAxis { get; private set; }

    public Vector2 AnalogAimAxis { get; private set; }

    public Vector2 MouseAimAxis { get; private set; }

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
        analogAimAction = InputSystem.actions.FindAction("AnalogAim");
        attackAction = InputSystem.actions.FindAction("Attack");
        healAction = InputSystem.actions.FindAction("Heal");
        pauseAction = InputSystem.actions.FindAction("Pause");
    }


    //update

    private void Update()
    {
        if (GameMarks.PlayerInputEnable)
        {
            MoveAxis = moveAction.ReadValue<Vector2>();
            AnalogAimAxis = analogAimAction.ReadValue<Vector2>();
            AttackTrigger = attackAction.IsPressed();
            HealTrigger = healAction.IsPressed();
            PauseTrigger = pauseAction.IsPressed();
        }
        else
        {
            MoveAxis = Vector2.zero;
            AnalogAimAxis = Vector2.zero;
            AttackTrigger = false;
            HealTrigger = false;
            PauseTrigger = false;
        }
    }

}
