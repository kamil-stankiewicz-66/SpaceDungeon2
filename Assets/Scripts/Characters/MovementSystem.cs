using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    Collider2D coll;

    Vector3 positionInPreviousFrame;


    private void Awake()
    {
        coll = GetComponent<Collider2D>();

        positionInPreviousFrame = transform.position;
    }


    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }

    public float RunSpeed
    {
        get => runSpeed;
        set => runSpeed = value;
    }

    public Vector3 MoveDirection { get; private set; }

    public bool IsMove { get => MoveDirection != Vector3.zero; }


    public void Move(Vector2 direction, float speed)
    {
        if (!GameMarks.EntitiesAIOn)
        {
            return;
        }

        direction.Normalize();
        float distance = Time.deltaTime * speed;

        Vector2 moveX = MoveHelper(direction * Vector2.right, distance);
        Vector2 moveY = MoveHelper(direction * Vector2.up, distance);

        transform.Translate(moveX);
        transform.Translate(moveY);
    }

    Vector2 MoveHelper(Vector2 direction, float distance)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = coll.Cast(direction, hits, distance, true);

        bool collision = hitCount > 0 && ((1 << hits[0].collider.gameObject.layer) & LAYER.RaycastWall) != 0;

        if (collision)
        {
            distance = Mathf.Max(0f, hits[0].distance - 0.01f);
        }

        return direction * distance;
    }


    private void Update()
    {
        Vector3 direction = transform.position - positionInPreviousFrame;
        
        if (direction != Vector3.zero)
        {
            direction.Normalize();
        }

        MoveDirection = direction;
        positionInPreviousFrame = transform.position;
    }
}
