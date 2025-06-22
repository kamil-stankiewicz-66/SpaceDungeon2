using UnityEngine;
public abstract class StoryActivity : MonoBehaviour
{
    private QuestManager questManager;
    private bool isCompleted;
    private int questID;

    
    //get and set

    public bool IsCompleted
    {
        get => isCompleted;
        set => isCompleted = value;
    }

    public int QuestID
    {
        get => questID;
        set => questID = value;
    }


    //abstract methods

    internal abstract string TaskContents();
    protected abstract bool ActivityPassingCondition();


    //private methods

    protected void OnEnable()
    {
        questManager = FindFirstObjectByType<QuestManager>().GetComponent<QuestManager>();
    }

    protected void Update()
    {
        if (isCompleted)
            return;

        if (ActivityPassingCondition())
        {
            isCompleted = true;
            if (questManager != null) questManager.DeleteQuestMark(questID);
            else Debug.LogWarning($"{gameObject.name} questManager == null");
        }
    }

}
