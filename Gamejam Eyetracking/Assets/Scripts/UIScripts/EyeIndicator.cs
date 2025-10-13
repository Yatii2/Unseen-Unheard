using UnityEngine;
using UnityEngine.UI;

public class EyeIndicator : MonoBehaviour
{
    public Image eyeOpenImage;    
    public Image eyeClosedImage;  

    void Update()
    {
        if (PlayerVisibility.Instance != null)
        {
            bool isVisible = PlayerVisibility.Instance.CurrentVisibility > 0.3f;

            eyeOpenImage.enabled = isVisible;
            eyeClosedImage.enabled = !isVisible;
        }
    }
}