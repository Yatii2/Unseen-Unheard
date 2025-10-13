using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class EchoProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    private int bounceCount = 0;
    private int maxBounces = 3;

    private SpriteRenderer sr;
    private Light2D lt; // Use Light2D for URP
    private AudioSource audioSource;

    [Header("Sound")]
    public AudioClip shootClip; // Assign this in the prefab!

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lt = GetComponent<Light2D>(); // Correct component type
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(Vector2 dir, float spd)
    {
        direction = dir.normalized;
        speed = spd;
        PlayShootSound();
    }

    public void SetColor(Color c)
    {
        if (sr != null)
            sr.color = c;
        if (lt != null)
            lt.color = c; // This will now work!
    }

    private void PlayShootSound()
    {
        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
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