using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject interactInfoBar;
    [SerializeField] UnityEvent[] interactionEvents;
    [SerializeField] bool isSingleUse;

    InputManager inputManager;
    new Collider2D collider;


    //state
    int _state = -1;
    public int State 
    {  
        get => _state;
        set
        {
            _state = value;

            if (_state >= interactionEvents.Length)
                _state = 0;

            InvokeInteraction(_state);
        }
    }


    //flag
    bool m_isTriggeredByPlayer;
    bool m_active; //anti loop activation



    //init

    private void Awake()
    {
        inputManager = FindAnyObjectByType<InputManager>();
        collider = GetComponent<Collider2D>();

        //test
        if (interactInfoBar == null) Debug.LogWarning("INTERACTABLE_OBJECT :: interact info bar is null");
        if (inputManager == null) Debug.LogWarning("INTERACTABLE_OBJECT :: input manager is null");
        if (collider == null) Debug.LogWarning("INTERACTABLE_OBJECT :: collider is null");

        //deafult
        interactInfoBar?.SetActive(false);
    }



    //collision trigger

    private void OnTriggerEnter2D(Collider2D collision) => CollisionTrigger(collision, true);

    private void OnTriggerExit2D(Collider2D collision) => CollisionTrigger(collision, false);

    void CollisionTrigger(Collider2D collision, bool trigger)
    {
        if (collision == null)
            return;

        if (!collision.CompareTag(TAG.PLAYER))
            return;

        if (!collision.gameObject.TryGetComponent<PlayerCore>(out _))
            return;

        if (IsInteractionAllowed())
        {
            m_isTriggeredByPlayer = trigger;
            interactInfoBar?.SetActive(trigger);
        }
        else
        {
            m_isTriggeredByPlayer = false;
        }
    }



    //loop

    private void Update()
    {
        if (!m_isTriggeredByPlayer || 
            !inputManager.InteractTrigger)
        {
            m_active = false; //anti loop activation
            return;
        }


        //anti loop activation
        if (m_active)
            return;

        m_active = true;


        //call interaction
        State++;        


        //bar
        interactInfoBar?.SetActive(IsInteractionAllowed());
    }



    //helpers

    bool IsInteractionAllowed()
    {
        return !isSingleUse || State == 0;
    }

    void InvokeInteraction(int state)
    {
        if (state < 0)
            return;

        if (interactionEvents.Length != 0)
            interactionEvents[State]?.Invoke();
    }
}
