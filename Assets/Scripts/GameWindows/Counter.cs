using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Counter : MonoBehaviour
{
    [SerializeField] GameObject counterHolder;
    [SerializeField] TextMeshProUGUI text_counter;
    bool isCounting;


    //public

    public void Call(
        ushort timeInSeconds,
        float? timeScaleAfter = null,
        UnityAction beforeCountingAction = null,
        UnityAction afterCountingAction = null)
    {
        if (isCounting)
        {
            return;
        }

        StartCoroutine(Count(timeInSeconds, timeScaleAfter, beforeCountingAction, afterCountingAction));
    }


    //private

    private IEnumerator Count(
        ushort time,
        float? _timeScaleAfter,
        UnityAction beforeCountingAction,
        UnityAction afterCountingAction)
    {
        isCounting = true;
        counterHolder.SetActive(true);
        beforeCountingAction?.Invoke();

        //before
        GameMarks.SetAll(false);
        float _timeBefore = Time.timeScale;

        if (Time.timeScale > 0.0f)
        {
            Time.timeScale = 0.0f;
        }

        //count
        for (int i = time; i > 0; i--)
        {
            text_counter.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        //after
        Time.timeScale 
            = _timeScaleAfter 
            ?? _timeBefore;
        GameMarks.SetAll(true);

        counterHolder.SetActive(false);
        afterCountingAction?.Invoke();
        isCounting = false;
    }

}
