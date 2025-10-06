using UnityEngine;

/// <summary>
/// Uitgebreide versie: loudness (zoals eerst) + pitch-detectie (autocorrelatie).
/// Loudness interface blijft hetzelfde.
/// </summary>
public class MicInputLevel : MonoBehaviour
{
    [Header("Loudness")]
    public float sensitivity = 100.0f;
    public int loudness = 0;
    public int loudnessSampleWindow = 128;

    [Header("Pitch (toonhoogte)")]
    public bool enablePitch = true;
    public int pitchSampleSize = 2048;
    public float pitchHz = 0f;              // ruwe pitch
    public float smoothedPitchHz = 0f;      // gesmoothte pitch
    public float pitchSmoothing = 8f;
    public float minPitch = 100f;           // laag (voor kleur mapping)
    public float maxPitch = 800f;           // hoog (voor kleur mapping)
    public float minDetectPitch = 60f;      // harde ondergrens detectie
    public float maxDetectPitch = 1200f;    // harde bovengrens detectie

    private AudioClip micClip;
    private string micName;

    // Buffers voor pitch
    private float[] pitchSamples;
    private float[] window; // Hanning window

    void Start()
    {
        // Use the default microphone
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 1, AudioSettings.outputSampleRate);

            if (enablePitch)
            {
                pitchSamples = new float[pitchSampleSize];
                window = new float[pitchSampleSize];
                BuildHanningWindow(window);
            }
        }
        else
        {
            Debug.LogWarning("Geen microfoon gedetecteerd!");
        }
    }

    void Update()
    {
        if (micClip == null) return;

        // Loudness (zoals je had)
        loudness = Mathf.RoundToInt(GetMaxVolume(loudnessSampleWindow) * sensitivity);

        // Pitch (nieuw)
        if (enablePitch)
        {
            loudness = GetMaxVolume() * sensitivity;
            Debug.Log("Sound Level: " + loudness.ToString("F2"));
            float detected = DetectPitchAutocorrelation();
            if (detected > 0f)
                pitchHz = detected;

            smoothedPitchHz = Mathf.Lerp(smoothedPitchHz, pitchHz, Time.deltaTime * pitchSmoothing);
        }
    }

    float GetMaxVolume(int sampleWindow)
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
        return maxVolume;
    }

    float DetectPitchAutocorrelation()
    {
        if (pitchSamples == null) return 0f;

        int micPos = Microphone.GetPosition(micName) - pitchSampleSize;
        if (micPos < 0) return 0f;

        micClip.GetData(pitchSamples, micPos);

        // Window toepassen
        for (int i = 0; i < pitchSampleSize; i++)
            pitchSamples[i] *= window[i];

        float sampleRate = AudioSettings.outputSampleRate;
        int minLag = Mathf.FloorToInt(sampleRate / maxDetectPitch);
        int maxLag = Mathf.CeilToInt(sampleRate / minDetectPitch);
        if (maxLag >= pitchSampleSize) maxLag = pitchSampleSize - 1;

        float bestCorr = 0f;
        int bestLag = 0;

        for (int lag = minLag; lag <= maxLag; lag++)
        {
            float sum = 0f;
            for (int i = 0; i < pitchSampleSize - lag; i++)
            {
                sum += pitchSamples[i] * pitchSamples[i + lag];
            }
            if (sum > bestCorr)
            {
                bestCorr = sum;
                bestLag = lag;
            }
        }

        if (bestLag == 0) return 0f;

        float freq = sampleRate / bestLag;
        if (freq < minDetectPitch || freq > maxDetectPitch)
            return 0f;

        return freq;
    }

    void BuildHanningWindow(float[] w)
    {
        int N = w.Length;
        for (int i = 0; i < N; i++)
        {
            w[i] = 0.5f * (1f - Mathf.Cos(2f * Mathf.PI * i / (N - 1)));
        }
    }

    public int GetLoudness() => loudness;

    // Genormaliseerde pitch 0..1 voor kleur
    public float GetNormalizedPitch()
    {
        if (!enablePitch) return 0f;
        return Mathf.Clamp01(Mathf.InverseLerp(minPitch, maxPitch, smoothedPitchHz));
    }
