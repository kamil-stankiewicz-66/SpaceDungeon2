using PlasticGui.WorkspaceWindow;
using UnityEngine;

[CreateAssetMenu(fileName = "SOCharDialSeq", menuName = "ScriptableObjects/Gameplay/SOCharacterDialogueSequence")]
public class SOCharDialSeq : ScriptableObject
{
    [System.Serializable] 
    public struct CharDialEntry
    {
        public Sprite profilePic;
        public string name;
        [TextArea(3, 10)] public string message;
    }

    [SerializeField] private CharDialEntry[] sequence;


    public CharDialEntry[] Get() => sequence;
}
