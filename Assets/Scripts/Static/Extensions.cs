using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class TransformExtensions
{
    public static void TurnAround(this Transform bodyToTransform, Vector2 axis, ref bool isFacingRight)
    {
        if (axis.x > 0 == isFacingRight)
            return;

        bodyToTransform.Rotate(0, 180, 0);
        isFacingRight = axis.x > 0;

    }

    public static void Aim(this Transform entityBody, ref Transform weaponHolder, Transform target, ref bool isFacingRight)
    {
        if (target == null)
        {
            weaponHolder.localEulerAngles = Vector2.zero;
            return;
        }

        entityBody.TurnAround(entityBody.DirectionTo(target), ref isFacingRight);
        Vector3 aimAngle = weaponHolder.AimRotationToTarget(target, isFacingRight);
        weaponHolder.eulerAngles = aimAngle;

    }

    public static Transform FindNearestTarget(this Transform entity, List<Transform> targets, LayerMask colliderLayer)
    {
        if (targets == null)
            return null;

        Transform nearestTarget = null;
        List<Transform> inVisionTargets = new List<Transform>(targets.Where(target => !entity.IsRycastHit(target, colliderLayer)));

        foreach (var target in inVisionTargets)
        {
            if (entity.DistanceToSqr(target) > entity.DistanceToSqr(nearestTarget ?? target))
                continue;

            nearestTarget = target;

        }

        return nearestTarget;
    }

    public static void Move(this Transform entity, Vector2 direction, float speed)
    {
        entity.Translate(direction * speed);

    }

    public static void MoveFixed(this Transform entity, Vector2 direction, float speed)
    {
        Vector2 fix = new Vector2(Mathf.Abs(direction.x), direction.y);
        entity.Translate(fix * speed);

    }

    public static void MoveNormalized(this Transform entity, Vector2 direction, float speed)
    {
        entity.Translate(direction.normalized * speed);

    }

    public static HashSet<Transform> GetAllChildsT(this Transform parent)
    {
        HashSet<Transform> childs = new HashSet<Transform>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform _child = parent.GetChild(i);

            if (_child.childCount > 0)
            {
                childs.UnionWith(_child.GetAllChildsT());
            }

            childs.Add(_child);
        }

        return childs;
    }

    public static List<GameObject> GetChilds(this Transform parent)
    {
        List<GameObject> childs = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject _child = parent.GetChild(i).gameObject;
            childs.Add(_child);
        }

        return childs;
    }

    public static void KillAllChilds(this Transform parent)
    {
        int _weaponHolderChildCount = parent.childCount;

        if (_weaponHolderChildCount > 0)
        {
            for (int i = 0; i < _weaponHolderChildCount; i++)
            {
                Object.Destroy(parent.GetChild(i).gameObject);
            }
        }
    }
}

public static class GameObjectExtensions
{
    public static HashSet<GameObject> GetAllChilds(this GameObject parent)
    {
        HashSet<GameObject> childs = new HashSet<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject _child = parent.transform.GetChild(i).gameObject;

            if (_child.transform.childCount > 0)
            {
                childs.UnionWith(_child.GetAllChilds());
            }

            childs.Add(_child);
        }

        return childs;
    }

    public static List<GameObject> GetAllChildsList(this GameObject parent)
    {
        List<GameObject> childs = new List<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject _child = parent.transform.GetChild(i).gameObject;

            if (_child.transform.childCount > 0)
            {
                childs.AddRange(_child.GetAllChildsList());
            }

            childs.Add(_child);
        }

        return childs;
    }
}

public static class AnimatorExtensions
{
    public static void ChangeAnimationState(this Animator animator, ref string currentAnimation, string newAnimation, float duration = 0f)
    {
        if (!animator.gameObject.activeSelf)
            return;

        if (newAnimation == currentAnimation)
            return;

        currentAnimation = newAnimation;
        animator.CrossFade(newAnimation, duration);
    }
}

public static class ImageExtensions
{
    public static void RefreshSlider(this Image image, float hp, float hp_max)
    {
        if (image == null)
            return;

        if (hp_max == 0)
        {
            Debug.Log($"{image.gameObject.name}: health system: slider; health_max == 0");
            return;
        }

        image.fillAmount = hp / hp_max;
    }
}

public static class Extensions
{
    public static void ChangeState(this ref ushort currentState, ushort targetState)
    {
        if (currentState == targetState)
            return;

        currentState = targetState;
    }

    public static string RemoveNums(this string text)
    {
        return Regex.Replace(text, @"\d", "").Trim();
    }

    public static string CreateText(TextMeshProUGUI txt, float stat) => $"{txt.text.RemoveNums()} {stat}";
    public static string CreateText(TextMeshProUGUI txt, int stat) => $"{txt.text.RemoveNums()} {stat}";
}

public static class TemplateExtensions
{
    /// <summary>
    /// Remove nulls in array.
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="baseArr">base array</param>
    /// <returns>new array</returns>
    public static T[] RemoveNulls<T>(this T[] baseArr) where T : Component
    {
        List<T> list = new List<T>();
        foreach (var item in baseArr)
        {
            if (item != null)
                list.Add(item);
        }

        T[] newArr = new T[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            T item = list[i];
            newArr[i] = item;
        }

        return newArr;
    }

    /// <summary>
    /// Universal getter for arrays.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T Get<T>(this T[] array, int index)
    {
        if (array == null)
        {
            Debug.LogWarning("EXTENSIONS :: GET<T> :: array is null");
            return default;
        }

        if (array.Length == 0)
        {
            Debug.LogWarning("EXTENSIONS :: GET<T> :: array is empty");
            return default;
        }

        if (index < 0 || array.Length <= index)
        {
            Debug.LogWarning("EXTENSIONS :: GET<T> :: index is out of range " + index);
            return default;
        }

        return array[index];
    }

    /// <summary>
    /// Short to get components
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="gameObject"></param>
    /// <returns>component</returns>
    public static T GetComponentEx<T>(this GameObject gameObject) where T : Component
    {
        return
            gameObject.GetComponent<T>()
            ?? gameObject.GetComponentInChildren<T>();
    }
}