using UnityEngine;
using UnityEngine.UI;

public class WebcamFollowPlayer : MonoBehaviour
{
    public Transform player; // Assign your player transform in Inspector

    void LateUpdate()
    {
        if (player != null)
        {
            // Use your desired offset if you want the webcam above the player, for example
            transform.position = player.position;
        }
    }
}