using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    /// <summary>
    /// After state change.
    /// </summary>

    public abstract void Init();


    /// <summary>
    /// Main action.
    /// </summary>

    public abstract void Execute();
}
