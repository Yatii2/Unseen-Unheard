using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Header("Enemy Settings")]
    public EnemyState currentState;
    public float chaseSpeed = 3f;
    public float chaseDistance = 5f;
    public float attackDistance = 1f;

    [Header("Mic Settings")]
    public MicInputLevel micInputLevel;
    public float detectionThreshold = 5.0f;

    [Header("Hearing")]
    public float hearingWindow = 0.5f;
    private float lastHeardTime = -Mathf.Infinity;

    [Header("Reset Sound")]
    public AudioClip resetSound;
    [Range(0f, 1f)] public float resetVolume = 1f;
    private AudioSource audioSource;

    private EnemyPathfinding pathfinding;

    private void Start()
    {
        currentState = EnemyState.Idle;
        pathfinding = GetComponent<EnemyPathfinding>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (micInputLevel != null && micInputLevel.loudness > detectionThreshold)
        {
            lastHeardTime = Time.time;
        }

        if (currentState == EnemyState.Idle && distanceToPlayer < chaseDistance && micInputLevel != null && micInputLevel.loudness > detectionThreshold)
        {
            currentState = EnemyState.Chase;
        }
        else if (currentState == EnemyState.Chase && distanceToPlayer >= chaseDistance)
        {
            currentState = EnemyState.Idle;
        }
        else if (currentState == EnemyState.Chase && distanceToPlayer < attackDistance)
        {
            currentState = EnemyState.Attack;
        }
        else if (currentState == EnemyState.Attack && distanceToPlayer >= attackDistance)
        {
            currentState = EnemyState.Chase;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                if (pathfinding != null)
                    pathfinding.Patrol();
                break;
            case EnemyState.Chase:
                EnemyChase();
                break;
            case EnemyState.Attack:
                break;
        }
    }

    void EnemyChase()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float rayDistance = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, LayerMask.GetMask("Wall"));
        if (hit.collider == null)
        {
            transform.position += direction * chaseSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player == null) return;

        if (other.gameObject == player.gameObject || other.CompareTag("Player"))
        {
            if (Time.time - lastHeardTime <= hearingWindow)
            {
                var playerScript = player.GetComponent<PlayerMovement>();
                if (playerScript != null)
                {
                    if (resetSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(resetSound, resetVolume);
                    }

                    playerScript.ResetToSpawn();
                    Debug.Log("EnemyAI: Player reset to spawn (heard recently)!");
                }
                else
                {
                    Debug.LogWarning("EnemyAI: Player script not found!");
                }
            }
            else
            {
                Debug.Log("EnemyAI: Collision with player but player was not heard recently — no reset.");
            }
        }
    }
}