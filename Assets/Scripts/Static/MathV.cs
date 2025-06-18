using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MathV
{
    public static bool IsRycastHit(this Transform entity, Transform target, LayerMask hitLayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(entity.position, entity.DirectionTo(target).normalized, entity.DistanceTo(target), hitLayer);
        return hit.collider != null;
    }

    public static bool IsRycastHit(this Vector3 entity, Vector3 target, LayerMask hitLayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(entity, (target - entity).normalized, Vector2.Distance(entity, target), hitLayer);
        return hit.collider != null;

    }

    public static string RycastHit(this Transform entity, Vector2 direction, float viewRange, LayerMask hitLayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(entity.position, direction, viewRange, hitLayer);
        return hit.collider.tag;

    }

    public static Vector3 AimRotationToTarget(this Transform entity, Transform target, bool _isFacingRight)
    {
        Vector3 direction = entity.DirectionTo(target).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (_isFacingRight)
            return new Vector3(0, 0, angle);
        else
            return new Vector3(180, 0, -angle);

    }

    public static Vector3 AimRotationToTarget(this Transform entity, Transform target)
    {
        Vector3 direction = entity.DirectionTo(target).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, angle);

    }

    public static void AddAngle(this ref Vector3 vector, float angle)
    {
        vector = new Vector3(vector.x, vector.y, vector.z + angle);
    }

    public static Vector2 DirectionTo(this Transform entity, Transform target) => target.position - entity.position;

    public static Vector2 DirectionTo(this Transform entity, Vector2 target) => target - (Vector2)entity.position;

    public static Vector2 DirectionTo(this Vector2 entity, Vector2 target) => target - entity;

    public static Vector2 DirectionToNormalized(this Transform entity, Transform target) => (target.position - entity.position).normalized;

    public static float DistanceToSqr(this Transform entity, Transform target) => (target.position - entity.position).sqrMagnitude;

    public static float DistanceTo(this Transform entity, Transform target) => (target.position - entity.position).magnitude;


    public static Vector3 ToAxisVector(this Quaternion quaternion)
    {
        float _y = quaternion.z * 0.0111111111111111f;
        float _x = 1 - _y;
        return new Vector3(_x, _y, 0);

    }

    public static float StatsLvlBonus(float basicStat, int level) => (float)(basicStat / 3 * level);

    public static float RandomAround(float value, int range = 4) => value + Random.Range(-value / range, value / range);

    public static float AnimationClipTime(this Animator animator, string animName)
    {
        float t = 0;
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips.Where(clip => clip.name == animName))
            t = clip.length;
        return t;
    }

    public static float RoundTo(this float number, ushort decimalPlaces)
    {
        float x = 1;
        for (int i = 1; i <= decimalPlaces; i++) x *= 10.0f;
        float n = number * x;
        return (int)n / x;
    }
}
