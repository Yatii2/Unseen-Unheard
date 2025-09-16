using UnityEngine;

public class MicInputLevel : MonoBehaviour
{
    public float sensitivity = 100.0f;
    public int loudness = 0; // Changed to int

    private AudioClip micClip;
    private string micName;
    private int sampleWindow = 128;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 1, AudioSettings.outputSampleRate);
        }
        else
        {
            Debug.LogWarning("No microphone detected!");
        }
    }

    void Update()
    {
        if (micClip != null)
        {
            loudness = Mathf.RoundToInt(GetMaxVolume() * sensitivity); // Rounded to nearest integer
        }
    }

    float GetMaxVolume()
    {
        float maxVolume = 0f;
        float[] waveData = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(micName) - (sampleWindow + 1);
        if (micPosition < 0) return 0;

        micClip.GetData(waveData, micPosition);

        for (int i = 0; i < sampleWindow; ++i)
        {
            float wavePeak = Mathf.Abs(waveData[i]);
            if (wavePeak > maxVolume) maxVolume = wavePeak;
        }

        return maxVolume;
    }

    public int GetLoudness()
    {
        return loudness;
    }
}
