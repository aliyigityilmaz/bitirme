using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string npcName;
    public DialogueEntry[] dialogueEntries;
}

[System.Serializable]
public class DialogueEntry
{
    [TextArea(3, 10)] public string dialogueLine;
    public DialogueResponse[] responses;
}

[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public ResponseType responseType;
    public Quest assignedQuest; // Eðer görev baþlatacaksa buraya eklenecek
}

public enum ResponseType
{
    Normal,      // Normal konuþma devam eder
    End,         // Konuþmayý bitirir
    AcceptQuest, // Görevi kabul eder
    CompleteQuest // Görevi tamamlar
}

