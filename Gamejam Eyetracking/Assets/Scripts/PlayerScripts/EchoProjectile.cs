using UnityEngine;

public class EchoProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    private int bounceCount = 0;
    private int maxBounces = 3;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector2 dir, float spd)
    {
        direction = dir.normalized;
        speed = spd;
    }

    public void SetColor(Color c)
    {
        if (sr != null)
            sr.color = c;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        if (direction.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
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
                Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}