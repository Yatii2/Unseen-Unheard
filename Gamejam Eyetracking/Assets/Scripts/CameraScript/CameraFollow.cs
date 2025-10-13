using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Assign the player here in the inspector

    void LateUpdate()
    {
        if (target != null)
        {
            // Keep the camera's z position unchanged so it doesn't end up inside your 2D scene
            Vector3 newPosition = target.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }
}