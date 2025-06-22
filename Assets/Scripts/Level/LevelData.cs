using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelData : MonoBehaviour
{
    //pointers
    [SerializeField] Transform spawnPoint;
    //holders
    [SerializeField] GameObject storyActivitiesHolder;
    [SerializeField] GameObject enemiesHolder;
    [SerializeField] GameObject chestHolder;

    //ram
    private StoryActivity[] storyActivities;
    private EntityCore[] enemies;
    private ChestCore[] chests;

    private void Awake()
    {
        BuildLevelData();
    }

    public void BuildLevelData()
    {
        if (spawnPoint == null)
            spawnPoint = GameObject.FindGameObjectWithTag(TAG.LEVEL_POINTERS_SPAWNPOINT).transform;

        if (storyActivitiesHolder == null)
            storyActivitiesHolder = GameObject.FindGameObjectWithTag(TAG.LEVEL_DATA_STORY_ACTIVITIES);

        if (enemiesHolder == null)
            enemiesHolder = GameObject.FindGameObjectWithTag(TAG.LEVEL_DATA_ENEMIES);

        if (chestHolder == null)
            chestHolder = GameObject.FindGameObjectWithTag(TAG.LEVEL_CHESTS);

        enemies = enemiesHolder.transform.GetComponentsInChildren<EntityCore>();
        storyActivities = storyActivitiesHolder.transform.GetComponentsInChildren<StoryActivity>();
        chests = chestHolder.transform.GetComponentsInChildren<ChestCore>();
    }


    /// <summary>
    /// spawnpoint
    /// </summary>
    /// <returns></returns>

    public Vector2 Spawnpoint
    {
        get => spawnPoint.position;
    }


    /// <summary>
    /// story
    /// </summary>
    /// <returns></returns>

    public StoryActivity[] StoryActivities
    {
        get
        {
            if (storyActivities == null)
                return new StoryActivity[0];

            return storyActivities;
        }
    }

    public int StoryActivitiesCompleted
    {
        get
        {
            if (storyActivities == null)
                return 0;

            int counter = storyActivities.Count(item => item.IsCompleted);
            return counter;
        }
    }

    public int StoryActivitiesCount
    {
        get
        {
            if (storyActivities == null)
                return 0;

            return storyActivities.Length;
        }
    }

    public bool IsLevelCompleted
    {
        get => StoryActivitiesCompleted == StoryActivitiesCount;
    }


    /// <summary>
    /// enemies
    /// </summary>

    public EntityCore[] Enemies
    {
        get
        {
            if (enemies == null)
                return new EntityCore[0];

            return enemies;
        }
    }

    public int EnemiesKilled
    {
        get
        {
            if (enemies == null)
                return 0;

            int counter = enemies.Count(item => item.GetComponent<HealthSystem>().Health <= 0);
            return counter;
        }
    }

    public int EnemiesKilledFixed
    {
        get
        {
            if (enemies == null)
                return 0;

            int counter = enemies.Count(
                item => 
                item.GetComponent<HealthSystem>().Health <= 0 
                || item.IsRespawnedInMaxingMode);

            return counter;
        }
    }

    public int EnemiesKilledFixedNegate
    {
        get
        {
            if (enemies == null)
                return 0;

            int counter = enemies.Count(
                item =>
                item.GetComponent<HealthSystem>().Health <= 0
                && !item.IsRespawnedInMaxingMode);

            return counter;
        }
    }

    public int EnemiesCount
    {
        get
        {
            if (enemies == null)
                return 0;

            return enemies.Length;
        }
    }

    /// <summary>
    /// chests
    /// </summary>
    
    public ChestCore[] Chests
    {
        get
        {
            if (chests == null)
                return new ChestCore[0];

            return chests;
        }
    }

    public int ChestsLooted
    {
        get
        {
            if (chests == null)
                return 0;

            int counter = chests.Count(item => item.IsLooted);
            return counter;
        }
    }

    public int ChestsLootedFixedNegate
    {
        get
        {
            if (chests == null)
                return 0;

            int counter = chests.Count(
                item =>
                item.IsLooted
                && !item.IsRespawnedInMaxingMode);

            return counter;
        }
    }

    public int ChestsCount
    {
        get
        {
            if (chests == null)
                return 0;

            return chests.Length;
        }
    }

}
