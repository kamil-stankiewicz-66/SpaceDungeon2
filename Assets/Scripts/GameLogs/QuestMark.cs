public class QuestMark : SystemLog
{
    public void Destroy()
    {
        StartCoroutine(NotificationLog_DestroyCor());
    }
}
