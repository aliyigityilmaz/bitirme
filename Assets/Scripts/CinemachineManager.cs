using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace DefaultNamespace
{
    public class CinematicManager : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;

        private void Start()
        {
            videoPlayer.loopPointReached += OnVideoEnded;
        }

        private void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                SkipCinematic();
            }
        }

        private void OnVideoEnded(VideoPlayer vp)
        {
            SkipCinematic();
        }

        private void SkipCinematic()
        {
           SceneManager.LoadScene("MainMenu");
        }
    }
}