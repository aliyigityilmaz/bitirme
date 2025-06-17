using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.Cinemachine; // Unity 6 Cinemachine

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineCamera cineCamera; // Unity 6 Cinemachine Camera
    public float zoomedFOV = 20f;
    public float normalFOV = 60f;
    public float zoomSpeed = 5f;

    private Coroutine currentZoom;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        TryAssignCinemachine();
    }

    public void ZoomIn()
    {
        if (currentZoom != null)
            StopCoroutine(currentZoom);

        currentZoom = StartCoroutine(ZoomToFOV(zoomedFOV));
    }

    public void ZoomOut()
    {
        if (currentZoom != null)
            StopCoroutine(currentZoom);

        currentZoom = StartCoroutine(ZoomToFOV(normalFOV));
    }

    private IEnumerator ZoomToFOV(float targetFOV)
    {
        if (cineCamera == null) yield break;

        float startFOV = cineCamera.Lens.FieldOfView;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * zoomSpeed;
            cineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        cineCamera.Lens.FieldOfView = targetFOV;
        currentZoom = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryAssignCinemachine();
    }

    private void TryAssignCinemachine()
    {
        // Yeni sahnede Cinemachine kamerayý bul
        cineCamera = FindObjectOfType<CinemachineCamera>();
        if (cineCamera == null)
        {
            Debug.LogWarning("No CinemachineCamera found in this scene.");
            return;
        }

        // Yeni sahnede player'ý bul ve target olarak ayarla
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            cineCamera.Follow = player.transform;
            cineCamera.LookAt = player.transform;

            // (Ýsteðe baðlý) Kamerayý baþlangýç pozisyonuna getir
            cineCamera.transform.position = player.transform.position + new Vector3(0, 5, -10);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
