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

    private EnemyPathfinding pathfinding;

    private void Start()
    {
        currentState = EnemyState.Idle;
        pathfinding = GetComponent<EnemyPathfinding>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentState == EnemyState.Idle && distanceToPlayer < chaseDistance && micInputLevel.loudness > detectionThreshold)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject && player.CompareTag("Player"))
        {
            var playerScript = player.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.ResetToSpawn();
                Debug.Log("EnemyAI: Player reset to spawn!");
            }
            else
            {
                Debug.LogWarning("EnemyAI: Player script not found!");
            }
        }
    }
}