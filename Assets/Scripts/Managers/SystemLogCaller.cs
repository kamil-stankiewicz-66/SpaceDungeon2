using UnityEngine;

public class SystemLogCaller : MonoBehaviour
{
    [SerializeField] protected GameObject log_holder;
    [SerializeField] protected GameObject log_prefab;


    /// <summary>
    /// public methods
    /// </summary>

    public void ShowLog(string _content)
    {
        GameObject _newNoticationLog = Instantiate(log_prefab, log_holder.transform);
        _newNoticationLog.GetComponent<SystemLog>().content_text.text = _content;
    }
}
