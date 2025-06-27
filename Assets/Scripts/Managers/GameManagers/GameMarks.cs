using System.Collections.Generic;
using UnityEngine;

public class GameMarks : MonoBehaviour
{
    static HashSet<int> activeMarks = new HashSet<int>();
    static int currentMarkId = 0;

    static int healthChangeEnable;
    static int healPointsChangeEnable;
    static int entitiesAIEnable;
    static int particlesEnable;
    static int playerInputEnable;
    static int canPlayerPauseGame;

    private void Awake()
    {
        healthChangeEnable = AddNewMark();
        healPointsChangeEnable = AddNewMark();
        entitiesAIEnable = AddNewMark();
        particlesEnable = AddNewMark();
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

    public static bool HealthChangeEnable
    {
        get => Get(healthChangeEnable);
        set => Set(healthChangeEnable, value);
    }

    public static bool HealPointsChangeEnable
    {
        get => Get(healPointsChangeEnable);
        set => Set(healPointsChangeEnable, value);
    }

    public static bool EntitiesAIEnable
    {
        get => Get(entitiesAIEnable);
        set => Set(entitiesAIEnable, value);
    }

    public static bool ParticlesEnable
    {
        get => Get(particlesEnable);
        set => Set(particlesEnable, value);
    }

    public static bool PlayerInputEnable
    {
        get => Get(playerInputEnable);
        set => Set(playerInputEnable, value);
    }

    public static bool CanPlayerPauseGame
    {
        get => Get(canPlayerPauseGame);
        set => Set(canPlayerPauseGame, value);
    }


    //func

    public static void SetAll(bool value)
    {
        for (int i = 0; i < currentMarkId; i++)
        {
            Set(i, value);
        }
    }
}
