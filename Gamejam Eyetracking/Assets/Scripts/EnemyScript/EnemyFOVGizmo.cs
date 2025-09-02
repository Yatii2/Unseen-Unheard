using UnityEngine;

public class EnemyFOVGizmo : MonoBehaviour
{
    public float fieldOfView = 90f;
    public float viewDistance = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 origin = transform.position;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, fieldOfView / 2f) * transform.right;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -fieldOfView / 2f) * transform.right;

        Gizmos.DrawLine(origin, origin + rightBoundary * viewDistance);
        Gizmos.DrawLine(origin, origin + leftBoundary * viewDistance);

        // Optional: Draw arc (approximate)
        for (float angle = -fieldOfView / 2f; angle < fieldOfView / 2f; angle += 5f)
        {
            Vector3 dir = Quaternion.Euler(0, 0, angle) * transform.right;
            Gizmos.DrawLine(origin, origin + dir * viewDistance);
        }
    }
}