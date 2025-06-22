using System;
using UnityEngine;

public class AIPerception : MonoBehaviour
{
    public bool SeeObject(Transform target)
    {
        Vector2 origin = transform.position;
        Vector2 destination = target.position;
        Vector2 direction = destination - origin;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, distance, LAYER.RaycastWall);

        return hit.collider == null;
    }

    public GameObject FindNearest<T>(T[] array, Func<T, bool> condition) where T : MonoBehaviour
    {
        T nearest = null;
        float minDist = float.MaxValue;

        foreach (T t in array)
        {
            if (!condition(t))
            {
                continue;
            }

            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = t;
            }
        }

        return nearest != null ? nearest.gameObject : null;
    }

    public GameObject FindNearest<T>(Func<T, bool> condition) where T : MonoBehaviour
    {
        T[] all = FindObjectsByType<T>(FindObjectsSortMode.None);
        return FindNearest(all, condition);
    }

}
