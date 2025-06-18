using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //core
    [SerializeField] PlayerCore playerCore;

    bool isMoving => playerCore.playerInput.MoveAxis != Vector2.zero;

    //anims
    string currentMoveAnimState;
    const string ANIM_IDLE = "Idle";
    const string RUN_ANIM = "Run";


    private void Update()
    {
        Vector2 _moveAxis = playerCore.playerInput.MoveAxis;

        if (isMoving)
        {
            playerCore.player.Move(_moveAxis, playerCore.speed * Time.deltaTime);

            if (!playerCore.isAiming)
                playerCore.playerBody.TurnAround(_moveAxis, ref playerCore.isFacingRight);

            //animation
            playerCore.moveAnim.ChangeAnimationState(ref currentMoveAnimState, RUN_ANIM, 0.1f);
        }
        else
        {
            //animation
            playerCore.moveAnim.ChangeAnimationState(ref currentMoveAnimState, ANIM_IDLE, 0.5f);

        }
    }

}
