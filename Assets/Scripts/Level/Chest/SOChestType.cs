using UnityEngine;

[CreateAssetMenu(fileName = "SOChestType", menuName = "ScriptableObjects/ChestType")]
public class SOChestType : ScriptableObject
{
    public Sprite closed;
    public Sprite openFull;
    public Sprite openEmpty;
}