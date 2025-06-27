using UnityEngine;

public abstract class Weapon : Item
{
    [SerializeField] float attackTimeOut;
    [SerializeField] float range;
    [SerializeField] float damage;

    protected Animator animator;
    protected const string ANIM = "Attack";

    float timeOut;
    float timeAcc;



    //property

    public float AttackTimeOut { get => attackTimeOut; }

    public float Range { get => range; }

    public float Damage { get => damage; }



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

        timeAcc = 0.0f;
        timeOut = MathV.RandomAround(AttackTimeOut, 10);

        AttackAction();

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.Play(ANIM);
        }
    }



    //abstract attack

    protected abstract void AttackAction();



    //engine    

    protected virtual void OnEnable()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
    }

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
