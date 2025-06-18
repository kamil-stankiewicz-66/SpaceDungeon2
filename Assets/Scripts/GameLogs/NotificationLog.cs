using System.Collections;
using UnityEngine;

public class NotificationLog : SystemLog
{
    [SerializeField] protected ushort lifeTime;
    protected const string ANIM_STATIC = "StaticAnim";


    /// <summary>
    /// overrides
    /// </summary>

    protected override IEnumerator NotificationLog_DestroyCor()
    {
        yield return new WaitForSecondsRealtime(lifeTime);
        yield return base.NotificationLog_DestroyCor();
    }

    protected override void OnEnable()
    {        
        base.OnEnable();
        StartCoroutine(NotificationLog_DestroyCor());
    }

    private void Start()
    {
        //jesli czas jest zatrzymany animacja wejscia jest pomijana
        if (Time.timeScale <= 0.0f)
        {
            animator?.Play(ANIM_STATIC);
        }
    }
}
