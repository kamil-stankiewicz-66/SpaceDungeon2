using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerEnemieDetector : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] GameObject levelHolder;
    [SerializeField] LayerMask aimCollider;
    private Camera mainCamera;
    private Transform[] allEnemies;
    private List<Transform> detectedEnemies;
    private Transform nearestEnemy;


    /// <summary>
    /// private
    /// </summary>

    public List<Transform> DetectedEnemies
    {
        get => detectedEnemies;
    }

    public Transform NearestEnemy
    {
        get => nearestEnemy;
    }


    /// <summary>
    /// private methods
    /// </summary>

    private void Awake()
    {
        mainCamera = Camera.main;
        detectedEnemies = new List<Transform>();

        LevelData levelData = levelHolder.GetComponentInChildren<LevelData>();
        allEnemies = new Transform[levelData.EnemiesAll];
        for (int i = 0; i < levelData.Enemies.Length; i++)
        {
            allEnemies[i] = levelData.Enemies[i].transform;
        }
    }

    private void Update()
    {
        detectedEnemies.Clear();
        detectedEnemies.AddRange(from Transform obj in allEnemies
                                 where IsObjectVisibleInCamera(obj.gameObject)
                                 select obj);

        nearestEnemy = transform.FindNearestTarget(detectedEnemies, aimCollider);

    }

    private bool IsObjectVisibleInCamera(GameObject obj)
    {
        if (!obj.activeSelf)
            return false;

        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            Bounds bounds = renderer.bounds;

            return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
        }

        // Jeœli obiekt nie ma komponentu Renderer, uznaj go za niewidoczny
        return false;
    }
}


//private void OnTriggerEnter2D(Collider2D collision)
//{
//    if (!collision.CompareTag(Tag.ENEMY))
//        return;

//    PlayerCore.detectedEnemies.Add(collision.gameObject.transform);
//}

//private void OnTriggerExit2D(Collider2D collision)
//{
//    if (!collision.CompareTag(Tag.ENEMY))
//        return;

//    PlayerCore.detectedEnemies.Remove(collision.gameObject.transform);
//}
