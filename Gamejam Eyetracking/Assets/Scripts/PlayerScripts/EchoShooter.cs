using UnityEngine;

public class EchoShooter : MonoBehaviour
{
    public GameObject echoProjectilePrefab;
    public MicInputLevel micInput;

    [Header("Shooting Settings")]
    public float minLoudness = 10f;         // threshold where it starts shooting
    public float minShootInterval = 0.12f;  // quiet: slower fire rate (~8 shots/sec)
    public float maxShootInterval = 0.02f;  // loud: very fast fire (~50 shots/sec)

    [Header("Projectile Speed")]
    public float baseSpeed = 10f;
    public float maxSpeed = 50f;            // louder voice = much faster projectiles

    [Header("Smoothing")]
    public float smoothingSpeed = 5f;       // higher = reacts quicker to volume

    private float shootTimer = 0f;
    private float smoothedLoudness = 0f;

    void Update()
    {
        if (micInput == null || echoProjectilePrefab == null || Camera.main == null)
            return;

        // Smooth loudness for gradual acceleration/deceleration
        smoothedLoudness = Mathf.Lerp(smoothedLoudness, micInput.loudness, Time.deltaTime * smoothingSpeed);

        if (smoothedLoudness >= minLoudness)
        {
            // normalize loudness 0–1
            float t = Mathf.InverseLerp(minLoudness, 100f, smoothedLoudness);

            // smoother fire rate scaling
            float currentInterval = Mathf.Lerp(minShootInterval, maxShootInterval, t);

            shootTimer += Time.deltaTime;
            if (shootTimer >= currentInterval)
            {
                ShootEcho(t);
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = 0f; // reset if quiet
        }
    }

    void ShootEcho(float t)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mouseWorldPos - transform.position).normalized;

        // speed scales smoothly with loudness
        float speed = Mathf.Lerp(baseSpeed, maxSpeed, t);

        GameObject projectile = Instantiate(echoProjectilePrefab, transform.position, Quaternion.identity);
        EchoProjectile echo = projectile.GetComponent<EchoProjectile>();
        if (echo != null)
        {
            echo.Initialize(shootDir, speed);
        }
    }
}
