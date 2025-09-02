using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public Transform player;
    public float attackDistance = 1f;
    public float fieldOfView = 90f;
    public float viewDistance = 5f;

    private float cantAttackTimer = 0f;
    private bool canAttack = true;
    private bool lastSeenPlayer = false;

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("EnemyAI: Player reference not set!");
            return;
        }

        float visibility = PlayerVisibility.Instance.CurrentVisibility;
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Field of view check
        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);
        bool playerInSight = (angleToPlayer < fieldOfView / 2f) && (distanceToPlayer < viewDistance);

        // Debug log when enemy sees or loses sight of player
        if (playerInSight && visibility >= 0.3f)
        {
            if (!lastSeenPlayer)
            {
                Debug.Log("EnemyAI: Enemy sees the player!");
                lastSeenPlayer = true;
            }
        }
        else
        {
            if (lastSeenPlayer)
            {
                Debug.Log("EnemyAI: Enemy lost sight of the player.");
                lastSeenPlayer = false;
            }
        }

        // If player is visible and in sight
        if (playerInSight && visibility >= 0.3f && canAttack)
        {
            // Seek and attack
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (distanceToPlayer < attackDistance)
            {
                AttackPlayer();
            }
        }
        else
        {
            // Avoid or ignore player for a few seconds
            if (playerInSight && visibility < 0.3f)
            {
                canAttack = false;
                cantAttackTimer = 2.0f; // Ignore for 2 seconds
                MoveAwayFromPlayer(directionToPlayer);
            }
        }

        // Timer for can't attack state
        if (!canAttack)
        {
            cantAttackTimer -= Time.deltaTime;
            if (cantAttackTimer <= 0f)
                canAttack = true;
        }
    }

    void AttackPlayer()
    {
        Debug.Log("EnemyAI: Enemy attacks player!");
        // Implement attack logic here
    }

    void MoveAwayFromPlayer(Vector2 direction)
    {
        Debug.Log("EnemyAI: Enemy moves away from player.");
        transform.position -= (Vector3)(direction * speed * Time.deltaTime);
    }
}