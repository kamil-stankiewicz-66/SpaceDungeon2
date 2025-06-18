using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float damageAdd;
    [SerializeField] protected float health_max;
    [SerializeField] protected int exp_level;
    [SerializeField] protected GameObject weapon;

    [SerializeField] protected AttackInvokeSystem attackInvoke;
    [SerializeField] protected HealthSystemEntity healthSystem;
    [SerializeField] protected Transform entity;
    [SerializeField] protected Transform entityRotationBody;
    [SerializeField] protected Transform weaponHolder;    
    [SerializeField] protected Animator moveAnimator;

    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected float rndMoveIntense;

    protected Weapon weaponCore;
    protected Transform player;

    protected const ushort AISTATE_IDLE = 2112;
    protected const ushort AISTATE_TRIGERRED = 2137;

    protected const string ANIM_IDLE = "Idle";
    protected const string ANIM_RUN = "Run";
    
    //storage
    protected Vector2 spawnPoint;
    protected string currentAnim = ANIM_IDLE;
    protected ushort aiState = AISTATE_IDLE;
    protected bool _isRespawnedInMaxingMode;

    public HealthSystemEntity HealthSystem
    {
        get => healthSystem;
    }

    public bool IsRespawnedInMaxingMode
    {
        get => _isRespawnedInMaxingMode;
        set => _isRespawnedInMaxingMode = value;
    }

    public int ExpLevel
    {
        get => exp_level;
        set
        {
            exp_level = value;
            damageAdd += MathV.StatsLvlBonus(damageAdd, exp_level);
            weaponCore.damage += MathV.StatsLvlBonus(weaponCore.damage, exp_level) + damageAdd;
            if (health_max <= 0) health_max = 1;
            healthSystem.Health_max = (int)(health_max + MathV.StatsLvlBonus(health_max, exp_level));
        } 
    }

    Vector3 prePos;
    protected bool IsMoving
    {
        get => transform.position != prePos;
    }

    protected bool isInRangeCircle(int _range)
    {
        return entity.DistanceTo(player) < _range;
    }

    protected bool isInRangeCircle(float _range)
    {
        return entity.DistanceTo(player) < _range;
    }

    protected Vector2 DirectionToPlayer => entity.DirectionTo(player.position);

    protected virtual void OnEnable()
    {
        spawnPoint = transform.position;
        prePos = spawnPoint;
        player = GameObject.FindGameObjectWithTag(TAG.PLAYER).transform;
        WeaponLoader.LoadWeaponData(weaponHolder, weapon, out weaponCore);
        ExpLevel = exp_level;
        speed = MathV.RandomAround(speed);

        //tag for elements
        foreach (var item in gameObject.GetAllChilds())
        {
            if (gameObject.CompareTag(item.tag))
                continue;
            item.tag = gameObject.tag;
        }
    }

    protected virtual void Update()
    {       
        //basic anims
        if (IsMoving) moveAnimator.ChangeAnimationState(ref currentAnim, ANIM_RUN, 0.4f);
        else moveAnimator.ChangeAnimationState(ref currentAnim, ANIM_IDLE, 0.4f);

        //save
        prePos = transform.position;

        //when idle
        if (aiState == AISTATE_IDLE)
        {
            if (!entity.IsRycastHit(player, LAYER.COLLIDER))
            {
                aiState.ChangeState(AISTATE_TRIGERRED);
            }
        }
        //when trigerred
        if (aiState == AISTATE_TRIGERRED)
        {
            if (entity.IsRycastHit(player, LAYER.COLLIDER))
            {
                aiState.ChangeState(AISTATE_IDLE);                
            }
        }

    }

    protected void MoveTo(Vector2 direction)
    {
        entity.MoveNormalized(direction, speed * Time.deltaTime);
        entityRotationBody.TurnAround(direction, ref isFacingRight);
    }

    protected void Goto(Vector2 target)
    {
        if (!entity.position.IsRycastHit(target, LAYER.COLLIDER))
        {
            MoveTo(entity.DirectionTo(target));
            return;
        }

        MoveRandom();

    }

    Vector2 rndDirection;
    float rnd;
    float nextTime = 0f;
    protected void MoveRandom()
    {
        if (Time.time >= nextTime)
        {
            rndDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rnd = MathV.RandomAround(rndMoveIntense, 2);
            nextTime = Time.time + rnd;
        }

        if (Physics2D.Raycast(entity.position, rndDirection, 2f, LAYER.COLLIDER).collider == null)
        {
            MoveTo(rndDirection);
        }
        else
        {
            nextTime = 0f;
        }

    }



}

