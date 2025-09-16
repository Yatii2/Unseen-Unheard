using UnityEngine;

public class MicInputLevel : MonoBehaviour
{
    public float sensitivity = 100.0f;
    public float loudness = 0.0f;

    private AudioClip micClip;
    private string micName;
    private int sampleWindow = 128;

    void Start()
    {
        // Use the default microphone
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
            loudness = GetMaxVolume() * sensitivity;
            Debug.Log("Sound Level: " + loudness.ToString("F2"));
        }
    }

    float GetMaxVolume()
    {
        float maxVolume = 0f;
        float[] waveData = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(micName) - (sampleWindow + 1);

        if (micPosition < 0) return 0;

        micClip.GetData(waveData, micPosition);
        // Find the max value in the wave data
        for (int i = 0; i < sampleWindow; ++i)
        {
            float wavePeak = Mathf.Abs(waveData[i]);
            if (wavePeak > maxVolume)
            {
                maxVolume = wavePeak;
            }
        }
        // Ignore noise floor
        if (maxVolume < 0.01f) maxVolume = 0f;
        return maxVolume;
    }
}