using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public int musicIndex; // AudioManager'daki m�zik listesine g�re m�zik ID'si

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.Instance.GetCurrentMusicIndex() != musicIndex)
            {
                AudioManager.Instance.PlayMusic(musicIndex);
            }
        }
    }
}
