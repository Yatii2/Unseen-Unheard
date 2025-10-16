using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public float soundCooldown = 0.5f; // Cooldown in seconds
    private float lastSoundTime = -999f;
    private AudioSource audioSource;

    void Start()
    {
        // Automatically get the AudioSource attached to the cube
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource attached to the cube!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EchoProjectile"))
        {
            Debug.Log("I've been hit!");
            if (audioSource != null && Time.time - lastSoundTime >= soundCooldown)
            {
                audioSource.Play();
                lastSoundTime = Time.time;
            }
        }
    }
}