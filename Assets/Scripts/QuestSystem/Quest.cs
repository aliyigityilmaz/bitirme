using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string questDescription;
    public bool isCompleted = false; // Görevin tamamlanıp tamamlanmadığını tutar
}
