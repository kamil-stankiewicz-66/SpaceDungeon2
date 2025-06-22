using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] GameObject questMark_holder;
    [SerializeField] GameObject questMark_prefab;
    [SerializeField] SystemLogCaller systemLogCaller;
    private Animator animator;

    private Dictionary<int, GameObject> questMarks;


    /// <summary>
    /// public methods
    /// </summary>

    public void CreateNewQuestMark(int quest_id, StoryActivity storyActivity)
    {
        if (questMarks.ContainsKey(quest_id))
            return;

        GameObject _newQuestMark = Instantiate(questMark_prefab, questMark_holder.transform);
        _newQuestMark.GetComponent<QuestMark>().content_text.text = storyActivity.TaskContents();
        questMarks.Add(quest_id, _newQuestMark);
    }

    public void DeleteQuestMark(int quest_id)
    {
        if (questMarks.TryGetValue(quest_id, out GameObject _questMark))
        {
            _questMark.GetComponent<QuestMark>().Destroy();
            questMarks.Remove(quest_id);
        }        

        if (questMarks.Count == 0)
        {
            systemLogCaller.ShowLog($"Story completed!");
        }
    }


    /// <summary>
    /// priv methods
    /// </summary>
    
    private void Awake()
    {
        questMarks = new Dictionary<int, GameObject>();
    }

    private void OnDestroy()
    {
        questMarks = null;
    }

}
