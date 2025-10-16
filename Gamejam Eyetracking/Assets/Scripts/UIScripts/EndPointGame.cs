using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointGame : MonoBehaviour
{
    public AudioClip completedSound;
    [Range(0f, 1f)] public float soundVolume = 1f;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (completedSound != null)
            {
                audioSource.PlayOneShot(completedSound, soundVolume);
            }
            SceneManager.LoadScene("EndScene");
        }
    }
}