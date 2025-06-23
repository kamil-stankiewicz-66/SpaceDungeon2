using UnityEngine;

public class DDOLSIObject : MonoBehaviour
{
    static GameObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = gameObject;
        DontDestroyOnLoad(instance);
    }
}
