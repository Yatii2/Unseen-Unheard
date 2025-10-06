using UnityEngine;

public class HitDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EchoProjectile"))
        {
            Debug.Log("I've been hit!");
        }
    }
}