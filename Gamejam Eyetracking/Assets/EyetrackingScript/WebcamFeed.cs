using UnityEngine;
using UnityEngine.UI;

public class WebcamFeed : MonoBehaviour
{
    public RawImage rawImage; // UI element to display webcam feed
    private WebCamTexture webCamTexture;

    void Start()
    {
        webCamTexture = new WebCamTexture();
        rawImage.texture = webCamTexture;
        rawImage.material.mainTexture = webCamTexture;
        webCamTexture.Play();
    }
}
