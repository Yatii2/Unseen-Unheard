using UnityEngine;
using UnityEngine.UI;

public class MicIndicator : MonoBehaviour
{
    public MicInputLevel mic;     
    public Image micOpen;    
    public Image micMuted;   

    public int threshold = 10;

    void Update()
    {
        if (mic == null) return;

        bool isOpen = mic.GetLoudness() >= threshold;

        micOpen.enabled = isOpen;
        micMuted.enabled = !isOpen;
    }
}