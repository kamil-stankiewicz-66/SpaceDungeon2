using System;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject rotationBody;

    const string ANIM_IDLE = "Idle";
    const string ANIM_RUN = "Run";

    Vector3 moveAxis;
    bool isFlipEnable;

    string anim_active;
    

    public void SetMoveAxis(Vector3 value) => moveAxis = value;

    public void EnableAutoFlip(bool value) => isFlipEnable = value;


    public void Flip(bool flip)
    {
        Vector3 scale = rotationBody.transform.localScale;
        scale.x = Math.Abs(scale.x);
        scale.x *= flip ? -1.0f : 1.0f;
        rotationBody.transform.localScale = scale;
    }


    private void Update()
    {
        bool _isMove = moveAxis != Vector3.zero;

        if (_isMove)
        {
            animator?.ChangeAnimationState(ref anim_active, ANIM_RUN, 0.4f);
        }
        else
        {
            animator?.ChangeAnimationState(ref anim_active, ANIM_IDLE, 0.4f);
        }

        if (isFlipEnable && moveAxis.x != 0.0f)
        {
            Flip(moveAxis.x < 0);
        }
    }

}
