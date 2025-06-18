using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] mobsPool;
    [SerializeField] [Min(0)] int mobsNr;
    [SerializeField] [Min(1)] int level;

    bool used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used) 
            return;

        if (!collision.CompareTag(TAG.PLAYER))
            return;

        Transform parentGameObj = GameObject.FindGameObjectWithTag(TAG.LEVEL_DATA_ENEMIES).transform;
        BoxCollider2D spawnZone = GetComponentInChildren<BoxCollider2D>();

        for (int nr = 0; nr < mobsNr; nr++)
        {
            GameObject entity = Instantiate(mobsPool[Random.Range(0, mobsPool.Length - 1)]);
            entity.transform.SetParent(parentGameObj);

            float posX = Random.Range(-spawnZone.size.x /2, spawnZone.size.x /2);
            float posY = Random.Range(-spawnZone.size.y /2, spawnZone.size.y /2);
            Vector2 pos = (Vector2)spawnZone.transform.position + spawnZone.offset + new Vector2(posX, posY);
            entity.transform.position = pos;

            Entity entityCore = entity.GetComponent<Entity>();
            entityCore.ExpLevel = level;
        }

        used = true;
    }

}
