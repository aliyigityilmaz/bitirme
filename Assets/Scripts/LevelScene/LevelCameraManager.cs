using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineCamera cineCamera; // Unity 6 Cinemachine
    public float zoomedFOV = 20f;
    public float normalFOV = 60f;
    public float zoomSpeed = 5f;

    private Coroutine currentZoom;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (cineCamera == null)
            Debug.LogError("Cinemachine Camera is not assigned!");
    }

    public void ZoomIn()
    {
        if (currentZoom != null)
        {
            StopCoroutine(currentZoom);
            currentZoom = null;
        }

        currentZoom = StartCoroutine(ZoomToFOV(zoomedFOV));
    }

    public void ZoomOut()
    {
        if (currentZoom != null)
        {
            StopCoroutine(currentZoom);
            currentZoom = null;
        }

        currentZoom = StartCoroutine(ZoomToFOV(normalFOV));
    }

    private IEnumerator ZoomToFOV(float targetFOV)
    {
        float startFOV = cineCamera.Lens.FieldOfView;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * zoomSpeed; // ZAMAN ÇARPANI DEÐÝÞTÝ
            cineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        cineCamera.Lens.FieldOfView = targetFOV;
        currentZoom = null;
    }

}
