public class MeleeCore : HandsCore
{
    //anim
    private const string ANIM = "Attack";

    public override void Attack()
    {
        base.Attack();
        animator.Play(ANIM);
    }


}
