using UnityEngine;
using TMPro;

public class NotificationCaller : MonoBehaviour
{
    [SerializeField] GameObject notificationHolder;
    [SerializeField] TextMeshProUGUI text_message;

    public void Show(string _message)
    {
        text_message.text = _message;

        if (notificationHolder.activeSelf)
            return;

        notificationHolder.SetActive(true);
    }

    public void Hide()
    {
        if (!notificationHolder.activeSelf)
            return;

        notificationHolder.SetActive(false);
    }

}
