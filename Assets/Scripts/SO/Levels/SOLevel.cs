using UnityEngine;

public enum ELevelState { Default, Started, Completed }

[CreateAssetMenu(fileName = "SOLevel", menuName = "ScriptableObjects/Levels/SOLevel")]
public class SOLevel : ScriptableObject
{
    [SerializeField] GameObject levelHolder;

    //meta data
    [SerializeField] ELevelState state;

    public GameObject Get
    {
        get => levelHolder;
    }

    public ELevelState State
    {
        get => state;
        set => state = value;
    }
}
