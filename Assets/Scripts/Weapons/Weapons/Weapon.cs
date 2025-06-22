using UnityEngine;

public abstract class Weapon : Item
{
    [SerializeField] protected Animator animator;
    [SerializeField] float attackTimeOut;
    [SerializeField] float range;
    [SerializeField] float damage;

    protected const string ANIM = "Attack";

    float timeOut;
    float timeAcc;



    //getters

    public float AttackTimeOut
    {
        get => attackTimeOut;
        set => attackTimeOut = value;
    }

    public float Range
    {
        get => range;
        set => range = value;
    }

    public float Damage
    {
        get => damage;
        set => damage = value;
    }



    //abstract aim

    public abstract void Aim(Vector2 target);



    //reset aim

    public void AimReset()
    {
        transform.rotation = Quaternion.identity;
    }



    //use implementation

    public override void Use()
    {
        if (!IsReadyToAttack())
        {
            return;
        }

        if (!GameMarks.ItemsOn)
        {
            return;
        }

        timeAcc = 0.0f;
        timeOut = MathV.RandomAround(attackTimeOut, 10);

        AttackAction();

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.Play(ANIM);
        }
    }



    //abstract attack

    protected abstract void AttackAction();



    //engine

    private void Update()
    {
        AccumulateTime(Time.deltaTime * 1000.0f);
    }



    //private helpers

    bool IsReadyToAttack()
    {
        return timeAcc >= timeOut;
    }

    void AccumulateTime(float miliseconds)
    {
        if (!IsReadyToAttack())
        {
            timeAcc += miliseconds;
        }
    }

}
