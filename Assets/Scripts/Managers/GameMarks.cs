using UnityEngine;

public class GameMarks : MonoBehaviour
{
    private static bool damageTakingOn;
    private static bool healTakingOn;
    private static bool healStaminaTakingOn;
    private static bool entitiesAIOn;
    private static bool weaponsOn;
    private static bool particlesOn;
    private static bool canPlayerPauseGame;


    /// <summary>
    /// get and set
    /// </summary>

    public static bool DamageTakingOn
    {
        get => damageTakingOn;
        set => damageTakingOn = value;
    }

    public static bool HealTakingOn
    {
        get => healTakingOn;
        set => healTakingOn = value;
    }

    
    public static bool HealStaminaTakingOn
    {
        get => healStaminaTakingOn;
        set => healStaminaTakingOn = value;
    }


    public static bool EntitiesAIOn
    {
        get => entitiesAIOn;
        set => entitiesAIOn = value;
    }


    public static bool WeaponsOn
    {
        get => weaponsOn;
        set => weaponsOn = value;
    }


    public static bool ParticlesOn
    {
        get => particlesOn;
        set => particlesOn = value;
    }


    public static bool CanPlayerPauseGame
    {
        get => canPlayerPauseGame;
        set => canPlayerPauseGame = value;
    }

    /// <summary>
    /// public methods
    /// </summary>

    public static void TurnAllOn()
    {
        DamageTakingOn = true;
        HealTakingOn = true;
        HealStaminaTakingOn = true;
        EntitiesAIOn = true;
        WeaponsOn = true;
        ParticlesOn = true;
        CanPlayerPauseGame = true;
    }

    public static void TurnAllOff()
    {
        DamageTakingOn = false;
        HealTakingOn = false;
        HealStaminaTakingOn = false;
        EntitiesAIOn = false;
        WeaponsOn = false;
        ParticlesOn = false;
        CanPlayerPauseGame = false;
    }
}
