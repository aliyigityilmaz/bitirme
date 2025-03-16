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
    public Quest assignedQuest; // E�er g�rev ba�latacaksa buraya eklenecek
}

public enum ResponseType
{
    Normal,      // Normal konu�ma devam eder
    End,         // Konu�may� bitirir
    AcceptQuest, // G�revi kabul eder
    CompleteQuest // G�revi tamamlar
}

