using UnityEngine;

public class EchoProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int maxBounces = 2;
    private int currentBounces = 0;
    private Vector2 direction;

    public void Initialize(Vector2 dir, float spd, int maxBounce)
    {
        direction = dir.normalized;
        speed = spd;
        maxBounces = maxBounce;
        currentBounces = 0;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = Vector2.Reflect(direction, collision.contacts[0].normal);
        currentBounces++;

        if (currentBounces > maxBounces)
        {
            Destroy(gameObject);
        }
    }
}