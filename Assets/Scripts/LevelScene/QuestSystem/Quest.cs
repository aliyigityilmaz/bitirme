using UnityEngine;

[CreateAssetMenu(menuName = "Quest/New Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
}
