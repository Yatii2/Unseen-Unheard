using UnityEngine;

public class EchoProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    private int bounceCount = 0;
    private int maxBounces = 3;

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Try different angle corrections to match your sprite's orientation
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + -90f, Vector3.forward); // Try +90f, or remove/add minus as needed
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 inDirection = direction;
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(inDirection, normal);
            bounceCount++;

            if (bounceCount > maxBounces)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}