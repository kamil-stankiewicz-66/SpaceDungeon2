using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class SystemLog : MonoBehaviour
{
    public TextMeshProUGUI content_text;
    protected Animator animator;
    protected float exitAnimLenght;

    protected const string ANIM_EXIT = "ExitAnim";


    //private methods

    protected virtual void OnEnable()
    {
        animator = GetComponent<Animator>();

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips.Where(clip => clip.name == ANIM_EXIT))
        {
            exitAnimLenght = clip.length;
        }
    }

    protected virtual IEnumerator NotificationLog_DestroyCor()
    {
        if (Time.timeScale > 0.0f)
        {
            animator?.Play(ANIM_EXIT);
            yield return new WaitForSeconds(exitAnimLenght);
        }

        Destroy(gameObject);
    }
}
