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
    EEntityState m_state;
    

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
        if (m_state == EEntityState.Seek)
        {
            if (AIPerception.SeeObject(player))
            {
                ChangeEntityState(EEntityState.Attack);
            }
            else
            {
                seekAbility?.Execute();
            }
        }
        //attack
        else if (m_state == EEntityState.Attack)
        {
            if (!AIPerception.SeeObject(player))
            {
                ChangeEntityState(EEntityState.Seek);
            }
            else
            {
                attackAbility?.Execute();
            }
        }
        //sleep
        else
        {
            if (AIPerception.SeeObject(player))
            {
                ChangeEntityState(EEntityState.Attack);
            }
            else
            {
                sleepAbility?.Execute();
            }
        }
    }

    void ChangeEntityState(EEntityState state)
    {
        if (m_state == state)
        {
            return;
        }


        m_state = state;
        
        switch (m_state)
        {
            case EEntityState.Sleep: sleepAbility?.Init(); break;
            case EEntityState.Seek: seekAbility?.Init(); break;
            case EEntityState.Attack: attackAbility?.Init(); break;
        }
    }

}
