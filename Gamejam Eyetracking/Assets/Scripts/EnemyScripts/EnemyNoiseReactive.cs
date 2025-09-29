using UnityEngine;
using System.Collections;

public class EnemyNoiseReactive : MonoBehaviour
{
    [SerializeField] private Transform player;
    private EnemyPathfinding pathfinding;
    private Vector3 currentDirection;

    [Header("Enemy Settings")]
    public EnemyState currentState;
    [SerializeField] private float runDuration = 2.0f;
    public float chaseSpeed = 4f;
    public float chaseDistance = 10f;
    public float attackDistance = 1f;
    private float runTimer;
    private Transform runTargetWaypoint;



    [Header("Mic Settings")]
    public MicInputLevel micInputLevel;
    public float detectionThreshold = 5.0f;
    void Start()
    {
        currentState = EnemyState.Idle;
        pathfinding = GetComponent<EnemyPathfinding>();
        currentDirection = Vector3.right;
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentState == EnemyState.Idle && distanceToPlayer < chaseDistance && micInputLevel.loudness < detectionThreshold)
        {
            currentState = EnemyState.Chase;
        }
        else if (currentState == EnemyState.Chase && distanceToPlayer <= chaseDistance && micInputLevel.loudness > detectionThreshold)
        {
            currentState = EnemyState.Run;
            runTimer = runDuration;
            runTargetWaypoint = FindNearestWaypoint();

            if (runTargetWaypoint != null && pathfinding != null)
            {
                for (int i = 0; i < pathfinding.patrolPoints.Length; i++)
                {
                    if (pathfinding.patrolPoints[i] == runTargetWaypoint)
                    {
                        pathfinding.currentPatrolIndex = i;
                        break;
                    }
                }
            }
        }
        if (currentState == EnemyState.Run)
        {
            runTimer -= Time.deltaTime;
            if (runTimer <= 0)
            {
                currentState = EnemyState.Chase;
            }
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
                // EnemyAttack();
                break;
            case EnemyState.Run:
                RunToWaypoint();
                break;
        }
    }


    void EnemyChase()
    {
        Vector3 targetDirection = (player.position - transform.position).normalized;
        float rayDistance = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, rayDistance, LayerMask.GetMask("Wall"));

        if (hit.collider == null)
        {
            currentDirection = Vector3.Lerp(currentDirection, targetDirection, Time.deltaTime * 5f); 
        }
        else
        {
            Vector3 wallNormal = hit.normal;
            Vector3 slideDirection = Vector3.Cross(wallNormal, Vector3.forward).normalized;
            currentDirection = Vector3.Lerp(currentDirection, slideDirection, Time.deltaTime * 5f); 
        }

        transform.position += currentDirection * chaseSpeed * Time.deltaTime;
    }


    //(EnemyAttack())
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player.gameObject && player.CompareTag("Player"))
        {
            var playerScript = player.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.ResetToSpawn();
                Debug.Log("EnemyNoiseReactive: Player reset to spawn!");
            }
            else
            {
                Debug.LogWarning("EnemyNoiseReactive: Player script not found!");
            }
        }
    }
    void RunToWaypoint()
    {
        if (runTargetWaypoint == null || pathfinding == null)
            return;

        Vector3 targetDirection = (runTargetWaypoint.position - transform.position).normalized;
        float rayDistance = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, rayDistance, LayerMask.GetMask("Wall"));

        if (hit.collider == null)
        {
            currentDirection = Vector3.Lerp(currentDirection, targetDirection, Time.deltaTime * 5f);
        }
        else
        {
            Vector3 wallNormal = hit.normal;
            Vector3 slideDirection = Vector3.Cross(wallNormal, Vector3.forward).normalized;
            currentDirection = Vector3.Lerp(currentDirection, slideDirection, Time.deltaTime * 2f);
        }

        transform.position += currentDirection * chaseSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, runTargetWaypoint.position) < pathfinding.waypointTolerance)
        {
            runTargetWaypoint = null;
        }

        if (runTargetWaypoint == null)
        {
            pathfinding.Patrol();
        }
    }
    Transform FindNearestWaypoint()
    {
        if (pathfinding == null || pathfinding.patrolPoints == null || pathfinding.patrolPoints.Length == 0)
            return null;

        Transform nearest = pathfinding.patrolPoints[0];
        float minDist = Vector3.Distance(transform.position, nearest.position);

        foreach (var wp in pathfinding.patrolPoints)
        {
            float dist = Vector3.Distance(transform.position, wp.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = wp;
            }
        }
        return nearest;
    }

}
