using UnityEngine;
using UnityEngine.Events;

public class AttackInvokeSystem : MonoBehaviour
{
    //bool
    private bool _canAttack = true;
    private float _time_start;
    private float _time_end;

    internal float TimeWaitingStart => _time_start;
    internal float TimeWaitingEnd => _time_end;
    

    internal void AttackInvoke(Weapon weapon,
                               UnityEvent customEvent = null)
    {
        if (!_canAttack)
            return;

        if (!GameMarks.WeaponsOn)
            return;

        //attack
        weapon?.Attack();
        customEvent?.Invoke();

        //wait for next attack
        _time_start = Time.time;
        _time_end = _time_start + (float)(weapon.attackTimeOut) / 1000.0f;
        _canAttack = false;
    }


    private void Update()
    {
        if (!GameMarks.WeaponsOn)
            return;

        if (_canAttack)
            return;

        if (Time.time >= _time_end)
        {
            _canAttack = true;
            return;
        }

    }

}
