using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum PatrolDirection { Horizontal, Vertical }
    public PatrolDirection patrolDirection = PatrolDirection.Horizontal;
    public float patrolDistance = 10f;
    public float speed = 2f;
    public Transform player; 

    private Vector2 startPos;
    private Vector2 leftLimit;
    private Vector2 rightLimit;
    private int direction = 1;

    public AudioClip resetSound;
    [Range(0f, 1f)] public float resetVolume = 1f;
    private AudioSource audioSource;

    void Start()
    {
        startPos = transform.position;

        if (patrolDirection == PatrolDirection.Horizontal)
        {
            leftLimit = startPos - new Vector2(patrolDistance, 0f);
            rightLimit = startPos + new Vector2(patrolDistance, 0f);
        }
        else // Vertical
        {
            leftLimit = startPos - new Vector2(0f, patrolDistance);
            rightLimit = startPos + new Vector2(0f, patrolDistance);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        void Update()
        {
            Patrol();
        }

        void Patrol()
        {
            if (patrolDirection == PatrolDirection.Horizontal)
            {
                transform.position += new Vector3(direction * speed * Time.deltaTime, 0f, 0f);

                if (direction == 1 && transform.position.x >= rightLimit.x)
                    direction = -1;
                else if (direction == -1 && transform.position.x <= leftLimit.x)
                    direction = 1;
            }
            else // Vertical
            {
                transform.position += new Vector3(0f, direction * speed * Time.deltaTime, 0f);

                if (direction == 1 && transform.position.y >= rightLimit.y)
                    direction = -1;
                else if (direction == -1 && transform.position.y <= leftLimit.y)
                    direction = 1;
            }
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