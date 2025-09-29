using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float waypointTolerance = 0.2f;

    public int currentPatrolIndex = 0;

    public void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetWaypoint = patrolPoints[currentPatrolIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        float rayDistance = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, LayerMask.GetMask("Wall"));
        if (hit.collider != null)
        {
            return;
        }

        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointTolerance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
}