using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerPerception : AIPerception
{
    LevelManager levelManager;


    public GameObject AttackTarget {  get; private set; }


    private void Awake()
    {
        levelManager = FindAnyObjectByType<LevelManager>();

        if (levelManager == null) Debug.LogWarning("PLAYER_PERCEPTION :: level manager not found");
    }


    private void Update()
    {
        AttackTarget = FindNearest(levelManager.ActiveLevel.Enemies,
            (EntityCore) => 
            { return 
                EntityCore.gameObject.activeSelf && 
                SeeObject(EntityCore.transform) && 
                IsObjectVisibleInCamera(EntityCore.gameObject); }
            );
    }


    bool IsObjectVisibleInCamera(GameObject obj)
    {
        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            Bounds bounds = renderer.bounds;

            return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
        }

        return false;
    }
}
