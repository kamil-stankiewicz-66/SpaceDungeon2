using UnityEngine;

public class SeekAbilityBasicRandom : Ability
{
    [SerializeField] float rndMoveIntense;

    Character character;
    Collider2D coll;

    Vector2 rndDirection;
    float nextTime = 0f;
    float timeAcc;


    private void OnEnable()
    {
        character = GetComponent<Character>();
        coll = GetComponent<Collider2D>();
    }

    public override void Init()
    {
        if (character.MovementSystem.MoveDirection != Vector3.zero)
        {
            rndDirection = character.MovementSystem.MoveDirection;
            UpdateNextTime();
        }
        else
        {
            SetRandom();
        }

        Weapon weapon = character.EquipmentSystem.ActiveItem as Weapon;
        weapon?.AimReset();
    }

    public override void Execute()
    {
        if (character.MovementSystem != null)
        {
            MoveRandom();
        }
    }

    public void MoveRandom()
    {
        timeAcc += Time.deltaTime;

        if (timeAcc >= nextTime)
        {
            SetRandom();
        }

        character.CharacterAnimationController?.EnableAutoFlip(true);
        character.CharacterAnimationController?.SetMoveAxis(character.MovementSystem.MoveDirection);
        character.MovementSystem.Move(rndDirection, character.MovementSystem.WalkSpeed);

        if (coll.IsTouchingLayers(LAYER.RaycastWall))
        {
            SetRandom();
        }
    }

    void SetRandom()
    {
        rndDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        UpdateNextTime();
    }

    void UpdateNextTime()
    {
        nextTime = MathV.RandomAround(rndMoveIntense, 2);
        timeAcc = 0f;
    }
}
