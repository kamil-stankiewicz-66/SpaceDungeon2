using UnityEngine;

public class KillAllEnemies : StoryActivity
{
    [SerializeField] private LevelData levelData;

    protected override bool ActivityPassingCondition()
    {
        return levelData.EnemiesCount - levelData.EnemiesKilled == 0;
    }

    internal override string TaskContents()
    {
        return "Clear the sector!";
    }
}
