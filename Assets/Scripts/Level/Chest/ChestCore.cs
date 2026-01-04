using System.Collections;
using UnityEngine;
using System.Linq;

public class ChestCore : LevelObject
{
    [SerializeField] SOChestType SO_chestType;
    [SerializeField] int coinsInChest;

    private PlayerCore playerCore;
    private SystemLogCaller systemLogCaller; 
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float triggerAnimLenght;
    private bool m_trigger_used;
    private bool _isRespawnedInMaxingMode;

    private const string ANIM_OPEN = "OpenAnim";
    private const string ANIM_TRIGGER = "TriggerAnim";

    public bool IsLooted
    {
        get => m_trigger_used;
        set
        {
            m_trigger_used = value;

            spriteRenderer.sprite = m_trigger_used
                ? SO_chestType.openEmpty
                : SO_chestType.closed;
        }
    }

    public bool IsRespawnedInMaxingMode
    {
        get => _isRespawnedInMaxingMode;
        set => _isRespawnedInMaxingMode = value;
    }

    private void OnEnable()
    {
        playerCore = GameObject.FindFirstObjectByType<PlayerCore>();
        systemLogCaller = GameObject.FindGameObjectWithTag(TAG.SYSTEM_LOG_CALLER).GetComponent<SystemLogCaller>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (playerCore == null) Debug.LogWarning("CHEST_CORE :: player core is null");
        if (systemLogCaller == null) Debug.LogWarning("CHEST_CORE :: systemLogCaller is null");
        if (animator == null) Debug.LogWarning("CHEST_CORE :: animator is null");
        if (spriteRenderer == null) Debug.LogWarning("CHEST_CORE :: spriteRenderer is null");

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips.Where(clip => clip.name == ANIM_TRIGGER))
        {
            triggerAnimLenght = clip.length;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_trigger_used)
            return;

        if (!collision.CompareTag(TAG.PLAYER))
            return;

        if (!collision.gameObject.TryGetComponent<PlayerCore>(out _))
            return;

        m_trigger_used = true;
        StartCoroutine(Open());
    }

    private IEnumerator Open()
    {
        //trigger
        animator?.Play(ANIM_TRIGGER);
        yield return new WaitForSeconds(triggerAnimLenght);

        //open
        spriteRenderer.sprite = SO_chestType.openEmpty;
        animator?.Play(ANIM_OPEN);

        //empty
        playerCore.EquipmentSystem.AddCoins(coinsInChest);
        systemLogCaller?.ShowLog($"+{coinsInChest} coins");
    }

}
