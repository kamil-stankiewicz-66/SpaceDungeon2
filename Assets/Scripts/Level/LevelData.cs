using UnityEngine;
using System.Linq;

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
    private Entity[] enemies;
    private ChestCore[] chests;

    private void OnEnable()
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

        enemies = enemiesHolder.transform.GetComponentsInChildren<Entity>();
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

            int counter = storyActivities.Count(item => item.IsActivityCompleted);
            return counter;
        }
    }

    public int StoryActivitiesAll
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
        get => StoryActivitiesCompleted == StoryActivitiesAll;
    }


    /// <summary>
    /// enemies
    /// </summary>

    public Entity[] Enemies
    {
        get
        {
            if (enemies == null)
                return new Entity[0];

            return enemies;
        }
    }

    public int EnemiesKilled
    {
        get
        {
            if (enemies == null)
                return 0;

            int counter = enemies.Count(item => item.HealthSystem.Health <= 0);
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
                item.HealthSystem.Health <= 0 
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
                item.HealthSystem.Health <= 0
                && !item.IsRespawnedInMaxingMode);

            return counter;
        }
    }

    public int EnemiesAll
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

    public int ChestsAll
    {
        get
        {
            if (chests == null)
                return 0;

            return chests.Length;
        }
    }

}
