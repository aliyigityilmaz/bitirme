using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public int musicIndex; // AudioManager'daki müzik listesine göre müzik ID'si

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
