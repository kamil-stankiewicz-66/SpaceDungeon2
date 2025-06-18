public class Zombie : Entity
{
    protected override void OnEnable() => base.OnEnable();

    //flags
    bool isFirstTriggerDone;

    protected override void Update()
    {
        //ai on check
        if (!GameMarks.EntitiesAIOn)
            return;

        base.Update();

        if (aiState == AISTATE_TRIGERRED)
        {
            isFirstTriggerDone = true;

            MoveTo(DirectionToPlayer);
            Attack();
        }

        if (aiState == AISTATE_IDLE)
        {
            if (isFirstTriggerDone)
                Goto(player.position);
        }
    }


    private void Attack()
    {
        if (isInRangeCircle(weaponCore.range))
        {
            attackInvoke?.AttackInvoke(weaponCore);
        }

    }


    

}
