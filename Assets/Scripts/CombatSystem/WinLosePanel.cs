using UnityEngine;
using UnityEngine.SceneManagement;
    public class WinLosePanel :MonoBehaviour
    {
        public GameObject winPanel;
        public GameObject losePanel;

        public void GoToOpenWorld()
        {
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            SceneManager.LoadScene("Open World Level");
        }
    }
