using UnityEngine;

public enum EEntityState { Sleep, Seek, Attack }

public class EntityCore : Character
{
    [SerializeField] Ability sleepAbility;
    [SerializeField] Ability seekAbility;
    [SerializeField] Ability attackAbility;

    //player
    Transform player;

    //ai state
    EEntityState state;
    

    //mode info

    bool _isRespawnedInMaxingMode;
    public bool IsRespawnedInMaxingMode
    {
        get => _isRespawnedInMaxingMode;
        set => _isRespawnedInMaxingMode = value;
    }


    //init

    protected override void OnEnable()
    {
        base.OnEnable();

        player = FindAnyObjectByType<PlayerCore>().transform;
    }


    //ai states update

    private void Update()
    {
        //seek
        if (state == EEntityState.Seek)
        {
            if (AIPerception.SeeObject(player))
            {
                state = EEntityState.Attack;
                attackAbility?.Init();
            }

            seekAbility?.Execute();
        }
        //attack
        else if (state == EEntityState.Attack)
        {
            if (!AIPerception.SeeObject(player))
            {
                state = EEntityState.Seek;
                seekAbility?.Init();
            }

            attackAbility?.Execute();
        }
        //sleep
        else
        {
            if (AIPerception.SeeObject(player))
            {
                state = EEntityState.Attack;
                attackAbility?.Init();
            }

            sleepAbility?.Execute();
        }
    }

}
