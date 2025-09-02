using UnityEngine;
using UnityEngine.UI;

public class WebcamFeed : MonoBehaviour
{
    public RawImage rawImage; // UI element to display webcam feed
    private WebCamTexture webCamTexture;

    [Range(0.01f, 1f)]
    public float sampleResolution = 0.1f;  // Lower = more samples, higher = faster

    void Start()
    {
        webCamTexture = new WebCamTexture();
        rawImage.texture = webCamTexture;
        rawImage.material.mainTexture = webCamTexture;
        webCamTexture.Play();
    }

    void Update()
    {
        if (webCamTexture.isPlaying && webCamTexture.didUpdateThisFrame)
        {
            float brightness = GetAverageBrightness(webCamTexture);
            float visibility = Mathf.InverseLerp(0.05f, 0.6f, brightness); // Adjust 0.05/0.6 as needed for min/max
            visibility = Mathf.Clamp01(visibility);

            float darknessPercent = (1.0f - brightness) * 100f;
            Debug.Log($"Brightness: {brightness:F2}, Darkness: {darknessPercent:F0}%, Visibility: {visibility:F2}");
            // Use `visibility` for your player logic!
        }
    }

    float GetAverageBrightness(WebCamTexture tex)
    {
        Color[] pixels = tex.GetPixels();
        int step = Mathf.Max(1, Mathf.FloorToInt(1f / sampleResolution));
        float total = 0f;
        int count = 0;

        for (int i = 0; i < pixels.Length; i += step)
        {
            Color c = pixels[i];
            // Perceived luminance formula: 0.299*R + 0.587*G + 0.114*B
            float luminance = 0.299f * c.r + 0.587f * c.g + 0.114f * c.b;
            total += luminance;
            count++;
        }

        return (count > 0) ? total / count : 0f;
    }
}