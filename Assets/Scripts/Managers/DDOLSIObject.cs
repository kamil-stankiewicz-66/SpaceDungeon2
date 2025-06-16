using UnityEngine;

public class DDOLSIObject : MonoBehaviour
{
    static GameObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = gameObject;
    }
}
