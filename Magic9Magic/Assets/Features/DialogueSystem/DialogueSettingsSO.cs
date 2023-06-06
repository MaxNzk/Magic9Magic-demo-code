using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSettings", menuName = "Dialogue/DialogueSettings")]
public class DialogueSettingsSO : ScriptableObject
{
    public bool IsFirstDialogue;
    public bool IsFirstDeath;
    public bool IsFirstDeathPlayed;
    public bool IsPickUpFirstItem;
}
