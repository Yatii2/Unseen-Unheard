using UnityEngine;

public class EnemyFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    public float visibleDuration = 2f;
    public float invisibleDuration = 2f;

    [Header("References")]
    public GameObject enemyVisual;
    public Transform player;

    [Header("Reset Sound")]
    public AudioClip resetSound;
    [Range(0f, 1f)] public float resetVolume = 1f;
    private AudioSource audioSource;

    private void Start()
    {
        if (enemyVisual == null)
        {
            Debug.LogError("EnemyFlickerRespawn: enemyVisual not assigned!");
            enabled = false;
            return;
        }


        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }

        StartCoroutine(FlickerLoop());
    }

    private System.Collections.IEnumerator FlickerLoop()
    {
        while (true)
        {
            // Enable enemy (visible)
            enemyVisual.SetActive(true);
            yield return new WaitForSeconds(visibleDuration);

            // Disable enemy (invisible)
            enemyVisual.SetActive(false);
            yield return new WaitForSeconds(invisibleDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player && PlayerVisibility.Instance.CurrentVisibility > 0.3f)
        {
            var playerScript = player.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                // Play the reset sound (if assigned) then reset the player
                if (resetSound != null)
                {
                    audioSource.PlayOneShot(resetSound, resetVolume);
                }

                playerScript.ResetToSpawn();
                Debug.Log("EnemyPatrolTrigger: Player reset to spawn!");
            }
            else
            {
                Debug.LogWarning("EnemyPatrolTrigger: Player script not found!");
            }
        }
    }
}