using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject interactInfoBar;
    [SerializeField] UnityEvent interactionEvent;

    InputManager inputManager;
    new Collider2D collider;



    //flag
    bool m_isInteractionCompleted;
    
    public bool IsTriggeredByPlayer { get; private set; }



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

        if (m_isInteractionCompleted)
            return;

        IsTriggeredByPlayer = trigger;
        interactInfoBar?.SetActive(trigger);
    }



    //loop

    private void Update()
    {
        if (!IsTriggeredByPlayer)
            return;

        //interact
        if (inputManager.InteractTrigger)
        {
            interactionEvent?.Invoke();
            interactInfoBar?.SetActive(false);
            m_isInteractionCompleted = true;
        }
    }
}
