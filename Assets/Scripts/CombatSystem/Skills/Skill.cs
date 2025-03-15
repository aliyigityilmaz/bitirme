using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public int baseDamage;

    public void Activate()
    {
        Debug.Log($"{skillName} activated!");
        // Skill'e ait diðer iþlemler burada gerçekleþtirilebilir.
    }
}
