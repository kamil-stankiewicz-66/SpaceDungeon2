using UnityEngine;
public abstract class StoryActivity : MonoBehaviour
{
    private QuestManager questManager;
    private bool _activityCompleted;
    private ushort _quest_id;

    
    /// <summary>
    /// get and set
    /// </summary>

    public bool IsActivityCompleted
    {
        get => _activityCompleted;
        set => _activityCompleted = value;
    }

    public ushort Quest_id
    {
        get => _quest_id;
        set => _quest_id = value;
    }


    /// <summary>
    /// abstract methods
    /// </summary>

    internal abstract string TaskContents();
    protected abstract bool ActivityPassingCondition();


    /// <summary>
    /// private methods
    /// </summary>

    protected void OnEnable()
    {
        questManager = FindFirstObjectByType<QuestManager>().GetComponent<QuestManager>();
    }

    protected void Update()
    {
        if (_activityCompleted)
            return;

        if (ActivityPassingCondition())
        {
            _activityCompleted = true;
            if (questManager != null) questManager.DeleteQuestMark(_quest_id);
            else Debug.LogWarning($"{gameObject.name} questManager == null");
        }
    }

}
