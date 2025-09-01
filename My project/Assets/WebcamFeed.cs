using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WebcamFeed : MonoBehaviour
{
    public RawImage rawImage;
    public int requestedWidth = 640;
    public int requestedHeight = 480;
    public int requestedFPS = 30;

    public WebCamTexture WebCamTex { get; private set; }
    public Texture2D FrameTex { get; private set; } // temp Texture2D for OpenCV conversion
    public bool IsReady => WebCamTex != null && WebCamTex.width > 16 && WebCamTex.height > 16;

    void Start()
    {
        var devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            // Use the first available camera
            WebCamTex = new WebCamTexture(devices[0].name, requestedWidth, requestedHeight, requestedFPS);
        }
        else
        {
            // No camera found – fallback (will probably just be a blank texture)
            Debug.LogWarning("No webcam detected. Using default WebCamTexture.");
            WebCamTex = new WebCamTexture(requestedWidth, requestedHeight, requestedFPS);
        }


        rawImage.texture = WebCamTex;
        WebCamTex.Play();

        // Texture2D used for CPU readback into OpenCV
        FrameTex = new Texture2D(requestedWidth, requestedHeight, TextureFormat.RGBA32, false);
    }

    void Update()
    {
        if (IsReady && rawImage.texture != WebCamTex)
            rawImage.texture = WebCamTex;
    }

    public Texture2D SnapshotToTexture2D()
    {
        if (!IsReady) return null;
        // Copy pixels into a Texture2D we can hand to OpenCV
        FrameTex.SetPixels32(WebCamTex.GetPixels32());
        FrameTex.Apply(false);
        return FrameTex;
    }
}
