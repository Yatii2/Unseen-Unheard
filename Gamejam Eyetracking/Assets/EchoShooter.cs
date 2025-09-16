using UnityEngine;

public class EchoShooter : MonoBehaviour
{
    public GameObject echoProjectilePrefab;
    public float minProjectileSpeed = 5f;
    public float maxProjectileSpeed = 20f;
    public float minFireInterval = 0.1f;
    public float maxFireInterval = 1.0f;
    public float loudnessThreshold = 0.1f;
    public int maxBounces = 2;

    private float fireTimer = 0f;
    private MicInputLevel micInput;

    void Start()
    {
        micInput = FindObjectOfType<MicInputLevel>();
        if (micInput == null)
        {
            Debug.LogError("MicInputLevel script niet gevonden!");
        }
        fireTimer = maxFireInterval;
    }

    void Update()
    {
        if (micInput == null) return;

        float loudness = micInput.GetLoudness();
        bool isLoud = loudness > loudnessThreshold;

        float t = Mathf.InverseLerp(loudnessThreshold, 1.0f, loudness);
        float fireInterval = Mathf.Lerp(maxFireInterval, minFireInterval, t);

        if (isLoud)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                FireEcho(loudness);
                fireTimer = 0f;
            }
        }
        else
        {
            fireTimer = fireInterval;
        }
    }

    void FireEcho(float loudness)
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 shooterPos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        Vector2 shootDir = (mousePos2D - shooterPos2D).normalized;

        float t = Mathf.InverseLerp(loudnessThreshold, 1.0f, loudness);
        float speed = Mathf.Lerp(minProjectileSpeed, maxProjectileSpeed, t);

        GameObject projectile = Instantiate(echoProjectilePrefab, shooterPos2D, Quaternion.identity);
        EchoProjectile echo = projectile.GetComponent<EchoProjectile>();
        if (echo != null)
        {
            echo.Initialize(shootDir, speed, maxBounces);
        }
    }
}