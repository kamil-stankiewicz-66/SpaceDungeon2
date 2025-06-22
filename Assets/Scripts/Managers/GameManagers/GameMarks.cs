using System.Collections.Generic;
using UnityEngine;

public class GameMarks : MonoBehaviour
{
    static HashSet<int> activeMarks = new HashSet<int>();
    static int currentMarkId = 0;

    static int healthChangeOn;
    static int healPointsChangeOn;
    static int entitiesAIOn;
    static int itemsOn;
    static int particlesOn;
    static int canPlayerPauseGame;

    private void Awake()
    {
        healthChangeOn = AddNewMark();
        healPointsChangeOn = AddNewMark();
        entitiesAIOn = AddNewMark();
        itemsOn = AddNewMark();
        particlesOn = AddNewMark();
        canPlayerPauseGame = AddNewMark();
    }

    static int AddNewMark()
    {
        return currentMarkId++;
    }

    static bool Get(int id) => activeMarks.Contains(id);
    static void Set(int id, bool value)
    {
        if (value) activeMarks.Add(id);
        else activeMarks.Remove(id);
    }

    public static bool HealthChangeOn
    {
        get => Get(healthChangeOn);
        set => Set(healthChangeOn, value);
    }

    public static bool HealPointsChangeOn
    {
        get => Get(healPointsChangeOn);
        set => Set(healPointsChangeOn, value);
    }

    public static bool EntitiesAIOn
    {
        get => Get(entitiesAIOn);
        set => Set(entitiesAIOn, value);
    }

    public static bool ItemsOn
    {
        get => Get(itemsOn);
        set => Set(itemsOn, value);
    }

    public static bool ParticlesOn
    {
        get => Get(particlesOn);
        set => Set(particlesOn, value);
    }

    public static bool CanPlayerPauseGame
    {
        get => Get(canPlayerPauseGame);
        set => Set(canPlayerPauseGame, value);
    }

    public static void SetAll(bool value)
    {
        for (int i = 0; i < currentMarkId; i++)
        {
            Set(i, value);
        }
    }
}
