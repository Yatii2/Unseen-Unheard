using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal; // For Light2D

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform spawnPoint;
    public Light2D playerLight; // Assign this in the inspector

    void Update()
    {

        if (PlayerVisibility.Instance != null)
        {
            bool tooDark = PlayerVisibility.Instance.CurrentVisibility <= 0.30f;
            if (playerLight != null)
                playerLight.enabled = !tooDark; 

            if (tooDark)
                return; // No movement allowed
        }

        Vector2 movement = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) movement.y += 1;
        if (Keyboard.current.sKey.isPressed) movement.y -= 1;
        if (Keyboard.current.aKey.isPressed) movement.x -= 1;
        if (Keyboard.current.dKey.isPressed) movement.x += 1;

        if (movement.sqrMagnitude > 1)
            movement = movement.normalized;

        transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;
    }

    public void ResetToSpawn()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("PlayerController: No spawn point assigned!");
        }
    }
}