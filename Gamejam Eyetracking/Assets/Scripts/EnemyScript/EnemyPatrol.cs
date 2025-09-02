using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform player; // Assign your player sprite here in Inspector
    public float patrolDistance = 10f;
    public float speed = 2f;
    public float fieldOfView = 90f;
    public float viewDistance = 5f;

    private Vector2 startPos;
    private Vector2 leftLimit;
    private Vector2 rightLimit;
    private int direction = 1; // 1 = right, -1 = left

    void Start()
    {
        startPos = transform.position;
        leftLimit = startPos - new Vector2(patrolDistance, 0f);
        rightLimit = startPos + new Vector2(patrolDistance, 0f);
    }

    void Update()
    {
        Patrol();

        bool visible = false;
        if (player != null)
        {
            visible = PlayerIsVisible();
        }

        // See if enemy sees the player and visibility is good
        if (visible && PlayerVisibility.Instance.CurrentVisibility > 0.3f)
        {
            ResetPlayerToSpawn();
        }
    }

    void Patrol()
    {
        transform.position += new Vector3(direction * speed * Time.deltaTime, 0f, 0f);

        if (direction == 1 && transform.position.x >= rightLimit.x)
            direction = -1;
        else if (direction == -1 && transform.position.x <= leftLimit.x)
            direction = 1;
    }

    bool PlayerIsVisible()
    {
        Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        float angleToPlayer = Vector2.Angle(Vector2.right * direction, directionToPlayer);
        bool inFOV = angleToPlayer < fieldOfView / 2f && distanceToPlayer < viewDistance;

        return inFOV;
    }

    void ResetPlayerToSpawn()
    {
        var playerScript = player.GetComponent<PlayerController>(); // Change PlayerController to your player script name
        if (playerScript != null)
        {
            playerScript.ResetToSpawn();
            Debug.Log("EnemyPatrol: Player reset to spawn!");
        }
        else
        {
            Debug.LogWarning("EnemyPatrol: Player script not found!");
        }
    }
}