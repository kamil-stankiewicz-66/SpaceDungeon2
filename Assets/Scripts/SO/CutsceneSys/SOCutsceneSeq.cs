using UnityEngine;

[CreateAssetMenu(fileName = "SOCharDialSeq", menuName = "ScriptableObjects/Gameplay/SOCharacterDialogueSequence")]
public class SOCutsceneSeq : ScriptableObject
{
    [System.Serializable] 
    public struct CutsceneStep
    {
        public SOCutsceneProfile profile;
        [TextArea(3, 10)] public string message;
    }

    [SerializeField] private CutsceneStep[] sequence;


    public CutsceneStep[] Get() => sequence;
}
