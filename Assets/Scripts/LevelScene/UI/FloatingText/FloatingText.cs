using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;
    private Color startColor;
    private float timer;

    void Start()
    {
        startColor = textMesh.color;
        timer = fadeDuration;
    }

    void Update()
    {
        // Yukarý doðru hareket
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Kamera yönüne dön
        transform.forward = Camera.main.transform.forward;

        // Saydamlaþma
        timer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(timer / fadeDuration);
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(string message, Color color)
    {
        textMesh.text = message;
        textMesh.color = color;
        startColor = color;
    }
}
